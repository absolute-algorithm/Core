using System.Collections.Generic;
using AbsoluteAlgorithm.Core.Models.Configuration;
using AbsoluteAlgorithm.Core.Models.Auth;
using AbsoluteAlgorithm.Core.Models.Caching;
using AbsoluteAlgorithm.Core.Models.Database;
using AbsoluteAlgorithm.Core.Models.Documentation;
using AbsoluteAlgorithm.Core.Models.Http;
using AbsoluteAlgorithm.Core.Models.Idempotency;
using AbsoluteAlgorithm.Core.Models.RateLimit;
using AbsoluteAlgorithm.Core.Models.Storage;
using AbsoluteAlgorithm.Core.Models.Webhooks;

namespace AbsoluteAlgorithm.Core.Builders;

/// <summary>
/// Builder class for constructing an ApplicationConfiguration object.
/// </summary>
public class ApplicationConfigurationBuilder
{
    private List<DatabasePolicy>? _databasePolicies;
    private List<StoragePolicy>? _storagePolicies;
    private List<HttpClientPolicy>? _httpClientPolicies;
    private ApiVersioningPolicy? _apiVersioningPolicy;
    private SwaggerPolicy? _swaggerPolicy;
    private IdempotencyPolicy? _idempotencyPolicy;
    private CachingPolicy? _cachingPolicy;
    private AuthManifest? _authManifest;
    private List<WebhookSignaturePolicy>? _webhookSignaturePolicies;
    private List<RateLimitPolicy>? _rateLimitPolicies;
    private bool _enableHealthChecks = true;
    private LoggingConfiguration? _loggingConfiguration;

    /// <summary>
    /// Sets the database policies for the application.
    /// </summary>
    /// <param name="policies">A list of database policies.</param>
    /// <returns>The current instance of ApplicationConfigurationBuilder.</returns>
    public ApplicationConfigurationBuilder WithDatabasePolicies(List<DatabasePolicy> policies)
    {
        _databasePolicies = policies;
        return this;
    }

    /// <summary>
    /// Sets the storage policies for the application.
    /// </summary>
    /// <param name="policies">A list of storage policies.</param>
    /// <returns>The current instance of ApplicationConfigurationBuilder.</returns>
    public ApplicationConfigurationBuilder WithStoragePolicies(List<StoragePolicy> policies)
    {
        _storagePolicies = policies;
        return this;
    }

    /// <summary>
    /// Sets the HTTP client policies for the application.
    /// </summary>
    /// <param name="policies">A list of HTTP client policies.</param>
    /// <returns>The current instance of ApplicationConfigurationBuilder.</returns>
    public ApplicationConfigurationBuilder WithHttpClientPolicies(List<HttpClientPolicy> policies)
    {
        _httpClientPolicies = policies;
        return this;
    }

    /// <summary>
    /// Enables or disables API versioning.
    /// </summary>
    /// <param name="enable">True to enable API versioning, false to disable.</param>
    /// <returns>The current instance of ApplicationConfigurationBuilder.</returns>
    public ApplicationConfigurationBuilder EnableApiVersioning(bool enable)
    {
        return this;
    }

    /// <summary>
    /// Sets the API versioning policy for the application.
    /// </summary>
    /// <param name="policy">The API versioning policy.</param>
    /// <returns>The current instance of ApplicationConfigurationBuilder.</returns>
    public ApplicationConfigurationBuilder WithApiVersioningPolicy(ApiVersioningPolicy policy)
    {
        _apiVersioningPolicy = policy;
        return this;
    }

    /// <summary>
    /// Enables or disables Swagger documentation.
    /// </summary>
    /// <param name="enable">True to enable Swagger, false to disable.</param>
    /// <returns>The current instance of ApplicationConfigurationBuilder.</returns>
    public ApplicationConfigurationBuilder EnableSwagger(bool enable)
    {
        return this;
    }

    /// <summary>
    /// Sets the Swagger policy for the application.
    /// </summary>
    /// <param name="policy">The Swagger policy.</param>
    /// <returns>The current instance of ApplicationConfigurationBuilder.</returns>
    public ApplicationConfigurationBuilder WithSwaggerPolicy(SwaggerPolicy policy)
    {
        _swaggerPolicy = policy;
        return this;
    }

    /// <summary>
    /// Enables or disables idempotency.
    /// </summary>
    /// <param name="enable">True to enable idempotency, false to disable.</param>
    /// <returns>The current instance of ApplicationConfigurationBuilder.</returns>
    public ApplicationConfigurationBuilder EnableIdempotency(bool enable)
    {
        return this;
    }

    /// <summary>
    /// Sets the idempotency policy for the application.
    /// </summary>
    /// <param name="policy">The idempotency policy.</param>
    /// <returns>The current instance of ApplicationConfigurationBuilder.</returns>
    public ApplicationConfigurationBuilder WithIdempotencyPolicy(IdempotencyPolicy policy)
    {
        _idempotencyPolicy = policy;
        return this;
    }

    /// <summary>
    /// Sets the shared caching policy for the application.
    /// </summary>
    /// <param name="policy">The caching policy.</param>
    /// <returns>The current instance of ApplicationConfigurationBuilder.</returns>
    public ApplicationConfigurationBuilder WithCachingPolicy(CachingPolicy policy)
    {
        _cachingPolicy = policy;
        return this;
    }

    /// <summary>
    /// Sets the authentication manifest for the application.
    /// </summary>
    /// <param name="manifest">The authentication manifest.</param>
    /// <returns>The current instance of ApplicationConfigurationBuilder.</returns>
    public ApplicationConfigurationBuilder WithAuthManifest(AuthManifest manifest)
    {
        _authManifest = manifest;
        return this;
    }

    /// <summary>
    /// Enables or disables webhook signature validation.
    /// </summary>
    /// <param name="enable">True to enable webhook signature validation, false to disable.</param>
    /// <returns>The current instance of ApplicationConfigurationBuilder.</returns>
    public ApplicationConfigurationBuilder EnableWebhookSignatureValidation(bool enable)
    {
        return this;
    }

    /// <summary>
    /// Sets the webhook signature policies for the application.
    /// </summary>
    /// <param name="policies">A list of webhook signature policies.</param>
    /// <returns>The current instance of ApplicationConfigurationBuilder.</returns>
    public ApplicationConfigurationBuilder WithWebhookSignaturePolicies(List<WebhookSignaturePolicy> policies)
    {
        _webhookSignaturePolicies = policies;
        return this;
    }

    /// <summary>
    /// Enables or disables rate limiting.
    /// </summary>
    /// <param name="enable">True to enable rate limiting, false to disable.</param>
    /// <returns>The current instance of ApplicationConfigurationBuilder.</returns>
    public ApplicationConfigurationBuilder EnableRateLimit(bool enable)
    {
        return this;
    }

    /// <summary>
    /// Sets the rate limit policies for the application.
    /// </summary>
    /// <param name="policies">A list of rate limit policies.</param>
    /// <returns>The current instance of ApplicationConfigurationBuilder.</returns>
    public ApplicationConfigurationBuilder WithRateLimitPolicies(List<RateLimitPolicy> policies)
    {
        _rateLimitPolicies = policies;
        return this;
    }

    /// <summary>
    /// Enables or disables health checks.
    /// </summary>
    /// <param name="enable">True to enable health checks, false to disable.</param>
    /// <returns>The current instance of ApplicationConfigurationBuilder.</returns>
    public ApplicationConfigurationBuilder EnableHealthChecks(bool enable)
    {
        _enableHealthChecks = enable;
        return this;
    }

    /// <summary>
    /// Sets the logging configuration for the application.
    /// </summary>
    /// <param name="loggingConfiguration">The logging configuration.</param>
    /// <returns>The current instance of ApplicationConfigurationBuilder.</returns>
    public ApplicationConfigurationBuilder WithLoggingConfiguration(LoggingConfiguration loggingConfiguration)
    {
        _loggingConfiguration = loggingConfiguration;
        return this;
    }

    /// <summary>
    /// Builds and returns the singleton instance of ApplicationConfiguration.
    /// </summary>
    /// <returns>The singleton instance of ApplicationConfiguration.</returns>
    public ApplicationConfiguration Build()
    {
        var instance = ApplicationConfiguration.Instance;

        instance.DatabasePolicies = _databasePolicies;
        instance.StoragePolicies = _storagePolicies;
        instance.HttpClientPolicies = _httpClientPolicies;
        instance.ApiVersioningPolicy = _apiVersioningPolicy;
        instance.SwaggerPolicy = _swaggerPolicy;
        instance.IdempotencyPolicy = _idempotencyPolicy;
        instance.CachingPolicy = _cachingPolicy;
        instance.AuthManifest = _authManifest;
        instance.WebhookSignaturePolicies = _webhookSignaturePolicies;
        instance.RateLimitPolicies = _rateLimitPolicies;
        instance.EnableHealthChecks = _enableHealthChecks;
        instance.LoggingConfiguration = _loggingConfiguration;

        return instance;
    }
}
