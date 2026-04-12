using System;
using System.Text.Json.Serialization;
using AbsoluteAlgorithm.Core.Models.Auth;
using AbsoluteAlgorithm.Core.Models.Caching;
using AbsoluteAlgorithm.Core.Models.Database;
using AbsoluteAlgorithm.Core.Models.Documentation;
using AbsoluteAlgorithm.Core.Models.Http;
using AbsoluteAlgorithm.Core.Models.Idempotency;
using AbsoluteAlgorithm.Core.Models.RateLimit;
using AbsoluteAlgorithm.Core.Models.Storage;
using AbsoluteAlgorithm.Core.Models.Webhooks;

namespace AbsoluteAlgorithm.Core.Models.Configuration;

/// <summary>
/// Represents the top-level configuration used to enable and configure library features.
/// Implements the Singleton design pattern.
/// </summary>
public class ApplicationConfiguration
{
    private static readonly Lazy<ApplicationConfiguration> _instance = new(() => new ApplicationConfiguration());

    /// <summary>
    /// Gets the singleton instance of the ApplicationConfiguration.
    /// </summary>
    public static ApplicationConfiguration Instance => _instance.Value;

    // Private constructor to prevent external instantiation
    private ApplicationConfiguration() { }

    /// <summary>
    /// Gets the database policies to register.
    /// </summary>
    [JsonPropertyName("databasePolicies")]
    public List<DatabasePolicy>? DatabasePolicies { get; set; }

    /// <summary>
    /// Gets the storage policies to register.
    /// </summary>
    [JsonPropertyName("storagePolicies")]
    public List<StoragePolicy>? StoragePolicies { get; set; }

    // --- HttpClient ---

    /// <summary>
    /// Gets the named HTTP client policies to register.
    /// </summary>
    [JsonPropertyName("httpClientPolicies")]
    public List<HttpClientPolicy>? HttpClientPolicies { get; set; }

    /// <summary>
    /// Gets the API versioning configuration.
    /// </summary>
    [JsonPropertyName("apiVersioningPolicy")]
    public ApiVersioningPolicy? ApiVersioningPolicy { get; set; }

    /// <summary>
    /// Gets the Swagger and OpenAPI documentation configuration.
    /// </summary>
    [JsonPropertyName("swaggerPolicy")]
    public SwaggerPolicy? SwaggerPolicy { get; set; }

    /// <summary>
    /// Gets the request idempotency configuration.
    /// </summary>
    [JsonPropertyName("idempotencyPolicy")]
    public IdempotencyPolicy? IdempotencyPolicy { get; set; }

    /// <summary>
    /// Gets the shared caching configuration for library features.
    /// </summary>
    [JsonPropertyName("cachingPolicy")]
    public CachingPolicy? CachingPolicy { get; set; }

    // --- Auth Sections ---
    /// <summary>
    /// Gets the authentication and authorization manifest.
    /// </summary>
    [JsonPropertyName("authManifest")]
    public AuthManifest? AuthManifest { get; set; }

    /// <summary>
    /// Gets the webhook signature validation policies.
    /// </summary>
    [JsonPropertyName("webhookSignaturePolicies")]
    public List<WebhookSignaturePolicy>? WebhookSignaturePolicies { get; set; }

    // --- RateLimit ---
    /// <summary>
    /// Gets the rate-limit policies to register.
    /// </summary>
    [JsonPropertyName("rateLimitPolicies")]
    public List<RateLimitPolicy>? RateLimitPolicies { get; set; }


    /// <summary>
    /// Gets a value indicating whether health endpoints are enabled.
    /// </summary>
    [JsonPropertyName("enableHealthChecks")]
    public bool EnableHealthChecks { get; set; } = true;

    /// <summary>
    /// Gets the request/response logging configuration.
    /// </summary>
    [JsonPropertyName("loggingConfiguration")]
    public LoggingConfiguration? LoggingConfiguration { get; set; }
}
