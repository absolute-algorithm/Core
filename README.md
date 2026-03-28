# AbsoluteAlgorithm.Core

**AbsoluteAlgorithm.Core** is a versatile core library designed to support a wide range of applications. It provides essential utilities and infrastructure for building robust and scalable applications.

## Features
- **API Versioning**: Simplifies version management for APIs.
- **Authentication Utilities**: Tools for secure authentication and token management.
- **Resilience Utilities**: Implements retry policies, rate limiting, and other resilience patterns.
- **Storage Providers**: Abstractions for various storage mechanisms.
- **Serialization**: JSON, XML, and compression utilities.
- **Security**: Hashing, encryption, and privacy utilities.
- **Sanitization**: Tools for sanitizing file names and spreadsheet formulas.

## Folder and Utility Overview

The following section describes the main classes and their functions in each of the core folders:

### Common

- **DateTimes**: Utility methods for working with `DateTime` (e.g., `EnsureUtc`, `StartOfDayUtc`, `EndOfDayUtc`, Unix time conversions, and range calculations).
- **Enums**: Helpers for enum parsing, name/value retrieval, and validation (`Parse`, `TryParse`, `GetNames`, `GetValues`, `IsDefined`).
- **TypeExtensions**: Reflection helpers for getting public/readable/writable properties of types.

### Concurrency

- **OptimisticConcurrencyUtility**: Methods for creating version tokens and ETags from row versions, timestamps, and for checking matches.
- **ETagUtility**: Methods to create strong/weak ETags from strings, bytes, or objects, and to apply ETags to HTTP responses.
- **FileUtility**: File-related helpers (get extension, file name, content type, format size, read all bytes/text from streams).

### Extensions

- **StringExtensions**: Extension methods for strings (truncate, convert to snake/kebab/pascal case, check palindrome).

### Networking

- **Connectivity**: Method to ping a host and check connectivity.
- **HttpUtility**: Helpers to extract headers, bearer tokens, client IP, correlation ID, tenant ID, and idempotency key from HTTP requests/contexts.
- **HttpRequestUtility**: Methods to create and send HTTP requests (including JSON/form requests), and helpers for common HTTP verbs.
- **RequestSignature**: Methods to generate timestamps, compute and verify request signatures.


### Numerics

- **Geometry**: Math helpers for interpolation, aspect ratio, area/volume calculations, and distance in 2D/3D.
- **Randomness**: Thread-safe random number generation, picking random items, and probability checks.
- **Statistics**: Statistical functions (mean, median, variance, standard deviation, range).
- **UniversalConverter**: Unit conversion utilities for length, liquid, mass, data, time, frequency, and temperature.
- **Percentage**: Generic percentage calculation utilities using .NET generic math (INumber/IFloatingPoint). Includes methods for calculating a percentage of a value, what percent one value is of another, increasing/decreasing by a percentage, and percentage difference.

### Resilience

- **ResiliencePolicyFactory**: Factory for creating async resilience policies (retry, circuit breaker, timeout) using Polly.

### Sanitizers

- **FileNameSanitizer**: Methods to sanitize file names and get safe file extensions.
- **SpreadsheetFormulaSanitizer**: Methods to sanitize spreadsheet cell formulas and check if sanitization is required.
- **SpreadsheetFormulaSanitizingStringModelBinder/Provider**: ASP.NET model binders for sanitizing spreadsheet formulas in string inputs.
- **SpreadsheetFormulaSanitizingStringJsonConverter**: JSON converter for sanitizing spreadsheet formulas.

### Security

- **Asymmetric**: Methods for generating asymmetric key pairs, private/public keys, and extracting public keys from private keys.
- **Hashing**: Methods for computing hashes (SHA256, SHA512, etc.) from strings, bytes, streams, and base64.
- **Identity**: Helpers for working with claims, user IDs, emails, roles, and creating claims/identities/principals.
- **Password**: Methods for hashing and verifying passwords.
- **Privacy**: Methods for masking sensitive information, detecting PII, and masking properties in JSON/objects.
- **Symmetric**: Methods for symmetric key generation, encryption, and decryption (string and byte array).
- **Token**: Methods for generating tokens, refresh tokens, one-time tokens, API keys, and symmetric keys.

### Serialization

- **Compression**: Methods for compressing/decompressing data using Gzip and Brotli.
- **Json**: Helpers for JSON serialization/deserialization and formatting.
- **Xml**: Helpers for XML serialization/deserialization and formatting.

---

## Getting Started
1. Install the package via NuGet:
   ```bash
   dotnet add package AbsoluteAlgorithm.Core
   ```
2. Add the necessary namespaces to your project.
3. Start using the utilities provided by the library.


## Sample Application Configuration

Below is a sample configuration for `ApplicationConfiguration` demonstrating how to set up databases, storage, rate limiting, API versioning, Swagger, authentication, and more:

```csharp
ApplicationConfiguration appConfig = new ApplicationConfiguration
{
    // Enable or disable relational database support (SQL Server, PostgreSQL)
    EnableRelationalDatabase = true,
    // List of database connection policies (multiple DBs supported)
    DatabasePolicies = new List<DatabasePolicy>
    {
        new DatabasePolicy
        {
            DatabaseProvider = DatabaseProvider.MSSQL,
            ConnectionStringName = "<MSSQL_CONNECTION_STRING_ENVIRONMENT_VARIABLE_NAME>",
            InitializeDatabase = true,
            InitializeAuditTable = true,
            InitializationScript = "<MSSQL_SCRIPT>",
            Name = "<KEYED_IDENTIFIER>",
            MaxPoolSize = 100,
            MinPoolSize = 10,
            CommandTimeoutSeconds = 30
        },
        new DatabasePolicy
        {
            DatabaseProvider = DatabaseProvider.PostgreSQL,
            ConnectionStringName = "<POSTGRESQL_CONNECTION_STRING_ENVIRONMENT_VARIABLE_NAME>",
            InitializeDatabase = true,
            InitializeAuditTable = true,
            InitializationScript = "<POSTGRESQL_SCRIPT>",
            Name = "<KEYED_IDENTIFIER>",
            MaxPoolSize = 100,
            MinPoolSize = 10,
            CommandTimeoutSeconds = 30
        }
    },
    EnableHealthChecks = true,
    EnableIdempotency = true,
    IdempotencyPolicy = new IdempotencyPolicy
    {
        ReplayableMethods = new List<string> { "POST", "PUT" },
        ExpirationMinutes = 10,
        IncludeQueryStringInKey = true,
        MaximumResponseBodyBytes = 1024 * 1024 // 1 MB
    },
    EnableStorage = true,
    StoragePolicies = new List<StoragePolicy>
    {
        new StoragePolicy
        {
            Name = "<KEYED_IDENTIFIER>",
            StorageProvider = StorageProvider.Minio,
            ConnectionStringName = "<MINIO_CONNECTION_STRING_ENVIRONMENT_VARIABLE_NAME>",
            BucketName = "<BUCKET_NAME>"
        },
        new StoragePolicy
        {
            Name = "<KEYED_IDENTIFIER>",
            StorageProvider = StorageProvider.AzureBlob,
            ConnectionStringName = "<AZURE_BLOB_STORAGE_CONNECTION_STRING_ENVIRONMENT_VARIABLE_NAME>",
            BucketName = "<BUCKET_NAME>"
        },
        new StoragePolicy
        {
            Name = "<KEYED_IDENTIFIER>",
            StorageProvider = StorageProvider.S3,
            ConnectionStringName = "<S3_STORAGE_CONNECTION_STRING_ENVIRONMENT_VARIABLE_NAME>",
            BucketName = "<BUCKET_NAME>"
        },
        new StoragePolicy
        {
            Name = "<KEYED_IDENTIFIER>",
            StorageProvider = StorageProvider.GoogleCloud,
            ConnectionStringName = "<GCP_STORAGE_CONNECTION_STRING_ENVIRONMENT_VARIABLE_NAME>",
            BucketName = "<BUCKET_NAME>"
        }
    },
    EnableRateLimit = true,
    RateLimitPolicies = new List<RateLimitPolicy>
    {
        new RateLimitPolicy
        {
            PolicyName = "<POLICY_NAME>",
            Algorithm = RateLimitAlgorithm.FixedWindow,
            Scope = RateLimitScope.IpAddress,
            PermitLimit = 2,
            Window = TimeSpan.FromSeconds(2)
        }
    },
    EnableApiVersioning = true,
    ApiVersioningPolicy = new ApiVersioningPolicy
    {
        DefaultMajorVersion = 1,
        DefaultMinorVersion = 0,
        AssumeDefaultVersionWhenUnspecified = true,
        ReportApiVersions = true,
        Readers = [
            ApiVersionReaderType.QueryString,
            ApiVersionReaderType.Header,
            ApiVersionReaderType.UrlSegment,
            ApiVersionReaderType.MediaType
        ],
        QueryStringParameterName = "api-version",
        HeaderNames = ["x-api-version"],
        MediaTypeParameterName = "ver"
    },
    EnableSwagger = true,
    SwaggerPolicy = new SwaggerPolicy
    {
        Title = "<API_TITLE>",
        Description = "<API_DESCRIPTION>",
        DocumentMode = SwaggerDocumentMode.PerApiVersion,
        Documents = new List<SwaggerDocumentDefinition>
        {
            new SwaggerDocumentDefinition { DocumentName = "v1", ApiGroupName = "v1", Version = "1.0", Title = "<DOC_TITLE_V1>" },
            new SwaggerDocumentDefinition { DocumentName = "v2", ApiGroupName = "v2", Version = "2.0", Title = "<DOC_TITLE_V2>" },
            new SwaggerDocumentDefinition { DocumentName = "v3", ApiGroupName = "v3", Version = "3.0", Title = "<DOC_TITLE_V3>" }
        }
    },
    EnableWebhookSignatureValidation = true,
    WebhookSignaturePolicies = new List<WebhookSignaturePolicy>
    {
        new WebhookSignaturePolicy
        {
            Name = "<WEBHOOK_POLICY_NAME>",
            PathPrefix = "<WEBHOOK_PATH_PREFIX>",
            SecretName = "<WEBHOOK_SECRET_ENVIRONMENT_VARIABLE_NAME>",
            Algorithm = RequestSignatureAlgorithm.HmacSha256,
            AllowedClockSkewSeconds = 300
        }
    },
    ConfigureAuthentication = true,
    ConfigureAuthorization = true,
    AuthManifest = new AuthManifest
    {
        EnableJwt = true,
        EnableCookies = true,
        EnableCsrfProtection = false,
        EnableApiKeyAuth = true,
        Policies = new List<AuthPolicy>
        {
            new AuthPolicy
            {
                PolicyName = "<AUTH_POLICY_NAME_1>",
                RequiredRoles = new List<string> { "<ROLE_1>", "<ROLE_2>" }
            },
            new AuthPolicy
            {
                PolicyName = "<AUTH_POLICY_NAME_2>",
                RequiredClaims = new Dictionary<string, string> { { "<CLAIM_KEY>", "<CLAIM_VALUE>" } }
            }
        }
    },
    LoggingConfiguration = new LoggingConfiguration
    {
        EnableLogging = true,
        EnablePiiRedaction = true,
        RedactedProperties = new List<string> { "Password", "Ssn", "Email" },
        IgnoredRoutes = new List<string> { "/health", "/login" },
        IgnoredLoggers = new List<string> { "SignalRHeartbeat" }
    },
};
```

## License
This project is licensed under the terms of the license specified in the [LICENSE.txt](LICENSE.txt) file.