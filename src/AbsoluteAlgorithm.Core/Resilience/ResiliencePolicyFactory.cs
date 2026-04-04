using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using AbsoluteAlgorithm.Core.Models.Resilience;
using AbsoluteAlgorithm.Core.Diagnostics;
using AbsoluteAlgorithm.Core.Enums;

namespace AbsoluteAlgorithm.Core.Resilience;

/// <summary>
/// A generic, decoupled factory to create Polly resilience policies.
/// Handles the "How" (delays, logging, wrapping) while letting the caller define the "What" (specific exceptions).
/// </summary>
public static class ResiliencePolicyFactory
{
    #region Generic Overloads (for methods returning <T>)

    /// <summary>
    /// Creates a generic asynchronous resilience policy for operations returning <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the result returned by the operation.</typeparam>
    /// <param name="policy">The resilience policy configuration.</param>
    /// <param name="shouldHandleResult">Predicate to determine if a result should be handled as a failure.</param>
    /// <param name="shouldHandleException">Optional predicate to determine if an exception should be handled.</param>
    /// <returns>An asynchronous policy for resilience handling.</returns>
    public static IAsyncPolicy<T> CreatePolicy<T>(
        ResiliencePolicy? policy, 
        Func<T, bool> shouldHandleResult,
        Func<Exception, bool>? shouldHandleException = null)
    {
        if (policy is null) return Policy.NoOpAsync<T>();

        var exceptionPredicate = shouldHandleException ?? (ex => true);

        return Policy.WrapAsync<T>(
            CreateTimeoutPolicy<T>(policy.Timeout),
            CreateCircuitBreakerPolicy(policy.CircuitBreaker, shouldHandleResult, exceptionPredicate),
            CreateRetryPolicy(policy.Retry, shouldHandleResult, exceptionPredicate));
    }

    private static AsyncRetryPolicy<T> CreateRetryPolicy<T>(
        RetryResiliencePolicy? retry,
        Func<T, bool> shouldHandleResult,
        Func<Exception, bool> shouldHandleException)
    {
        if (retry is null || retry.MaxRetryAttempts <= 0)
            return Policy.HandleResult<T>(_ => false).RetryAsync(0);

        return Policy<T>
            .Handle<Exception>(ex => shouldHandleException(ex))
            .OrResult(shouldHandleResult)
            .WaitAndRetryAsync(
                retry.MaxRetryAttempts,
                retryAttempt => CreateDelay(retry, retryAttempt),
                onRetry: (result, timeSpan, attempt, context) =>
                {
                    Logger.Warn($"Resilience: Attempt {attempt} failed. Retrying in {timeSpan.TotalMilliseconds}ms. Op: {context.OperationKey}");
                });
    }

    private static AsyncCircuitBreakerPolicy<T> CreateCircuitBreakerPolicy<T>(
        CircuitBreakerResiliencePolicy? circuitBreaker, 
        Func<T, bool> shouldHandleResult,
        Func<Exception, bool> shouldHandleException)
    {
        if (circuitBreaker is null || circuitBreaker.HandledEventsAllowedBeforeBreaking <= 0)
            return Policy<T>.HandleResult(_ => false).CircuitBreakerAsync(100_000, TimeSpan.FromMilliseconds(1));

        return Policy<T>
            .Handle<Exception>(ex => shouldHandleException(ex))
            .OrResult(shouldHandleResult)
            .CircuitBreakerAsync(
                circuitBreaker.HandledEventsAllowedBeforeBreaking,
                TimeSpan.FromSeconds(Math.Max(1, circuitBreaker.DurationOfBreakSeconds)),
                onBreak: (res, span, ctx) => Logger.Error($"Circuit BROKEN for {span.TotalSeconds}s. Op: {ctx.OperationKey}"),
                onReset: (ctx) => Logger.Info($"Circuit RESET. Op: {ctx.OperationKey}"));
    }

    private static IAsyncPolicy<T> CreateTimeoutPolicy<T>(TimeoutResiliencePolicy? timeout)
    {
        if (timeout is null || timeout.TimeoutSeconds <= 0) return Policy.NoOpAsync<T>();
        return Policy.TimeoutAsync<T>(TimeSpan.FromSeconds(timeout.TimeoutSeconds), TimeoutStrategy.Optimistic);
    }

    #endregion

    #region Non-Generic Overloads (for Task/void methods)

    /// <summary>
    /// Creates a non-generic asynchronous resilience policy for void (Task) operations.
    /// </summary>
    public static IAsyncPolicy CreatePolicy(
        ResiliencePolicy? policy, 
        Func<Exception, bool>? shouldHandleException = null)
    {
        if (policy is null) return Policy.NoOpAsync();

        var exceptionPredicate = shouldHandleException ?? (ex => true);

        return Policy.WrapAsync(
            CreateTimeoutPolicy(policy.Timeout),
            CreateCircuitBreakerPolicy(policy.CircuitBreaker, exceptionPredicate),
            CreateRetryPolicy(policy.Retry, exceptionPredicate));
    }

    private static AsyncRetryPolicy CreateRetryPolicy(RetryResiliencePolicy? retry, Func<Exception, bool> shouldHandleException)
    {
        if (retry is null || retry.MaxRetryAttempts <= 0)
            return Policy.Handle<Exception>(_ => false).RetryAsync(0);

        return Policy
            .Handle<Exception>(ex => shouldHandleException(ex))
            .WaitAndRetryAsync(
                retry.MaxRetryAttempts,
                retryAttempt => CreateDelay(retry, retryAttempt),
                onRetry: (ex, timeSpan, attempt, context) =>
                {
                    Logger.Warn($"Resilience: Attempt {attempt} failed. Retrying in {timeSpan.TotalMilliseconds}ms. Op: {context.OperationKey}");
                });
    }

    private static AsyncCircuitBreakerPolicy CreateCircuitBreakerPolicy(CircuitBreakerResiliencePolicy? circuitBreaker, Func<Exception, bool> shouldHandleException)
    {
        if (circuitBreaker is null || circuitBreaker.HandledEventsAllowedBeforeBreaking <= 0)
            return Policy.Handle<Exception>(_ => false).CircuitBreakerAsync(100_000, TimeSpan.FromMilliseconds(1));

        return Policy
            .Handle<Exception>(ex => shouldHandleException(ex))
            .CircuitBreakerAsync(
                circuitBreaker.HandledEventsAllowedBeforeBreaking,
                TimeSpan.FromSeconds(Math.Max(1, circuitBreaker.DurationOfBreakSeconds)),
                onBreak: (ex, span, ctx) => Logger.Error($"Circuit BROKEN for {span.TotalSeconds}s. Op: {ctx.OperationKey}"),
                onReset: (ctx) => Logger.Info($"Circuit RESET. Op: {ctx.OperationKey}"));
    }

    private static IAsyncPolicy CreateTimeoutPolicy(TimeoutResiliencePolicy? timeout)
    {
        if (timeout is null || timeout.TimeoutSeconds <= 0) return Policy.NoOpAsync();
        return Policy.TimeoutAsync(TimeSpan.FromSeconds(timeout.TimeoutSeconds), TimeoutStrategy.Optimistic);
    }

    #endregion

    #region Shared Private Helpers

    private static TimeSpan CreateDelay(RetryResiliencePolicy retry, int retryAttempt)
    {
        if (retry.DelayScheduleMilliseconds is { Count: > 0 } schedule)
        {
            var index = Math.Min(Math.Max(0, retryAttempt - 1), schedule.Count - 1);
            return TimeSpan.FromMilliseconds(Math.Max(1, schedule[index]));
        }

        var strategy = retry.DelayStrategy ?? (retry.UseExponentialBackoff ? RetryDelayStrategy.Exponential : RetryDelayStrategy.Fixed);
        var baseDelay = Math.Max(1, retry.DelayMilliseconds);

        return strategy switch
        {
            RetryDelayStrategy.Fixed => TimeSpan.FromMilliseconds(baseDelay),
            RetryDelayStrategy.Linear => TimeSpan.FromMilliseconds(baseDelay + (retryAttempt - 1) * Math.Max(1, retry.DelayIncrementMilliseconds)),
            RetryDelayStrategy.Exponential => TimeSpan.FromMilliseconds(baseDelay * Math.Pow(retry.BackoffMultiplier > 1d ? retry.BackoffMultiplier : 2d, retryAttempt - 1)),
            _ => TimeSpan.FromMilliseconds(baseDelay)
        };
    }

    #endregion
}