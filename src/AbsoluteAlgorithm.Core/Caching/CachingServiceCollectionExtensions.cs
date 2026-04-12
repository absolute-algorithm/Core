using AbsoluteAlgorithm.Core.Models.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace AbsoluteAlgorithm.Core.Caching;

/// <summary>
/// Provides dependency-injection registration helpers for caching services.
/// </summary>
public static class CachingServiceCollectionExtensions
{
    /// <summary>
    /// Registers the default caching services used by the library.
    /// </summary>
    /// <remarks>
    /// The current implementation supports in-process memory caching.
    /// </remarks>
    public static IServiceCollection AddAbsoluteAlgorithmCaching(this IServiceCollection services, CachingPolicy? policy)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (policy is null || !policy.Enabled)
        {
            return services;
        }

        services.AddMemoryCache();
        services.AddSingleton(policy);
        services.AddSingleton<IApplicationCache, MemoryApplicationCache>();
        services.AddSingleton<IIdempotencyCacheStore, IdempotencyCacheStore>();

        return services;
    }
}
