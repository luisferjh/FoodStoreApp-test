using System.Text;
using codeessentials.Extensions.AI.OpenApi;
using codeessentials.Extensions.AI.OpenApi.Model;
using Microsoft.Extensions.AI;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Swagger;

namespace OnlineStoreApp.Extensions
{
    /// <summary>
    /// Describes an OpenAPI operation that either could not be turned into an <see cref="AITool"/>
    /// or was mapped using a fallback strategy that deserves attention.
    /// </summary>
    /// <param name="Path">The OpenAPI path template of the operation.</param>
    /// <param name="HttpMethod">The HTTP method of the operation.</param>
    /// <param name="OperationId">The operationId declared in the spec, if any.</param>
    /// <param name="Reason">Why the operation was flagged.</param>
    /// <param name="ToolCreated">Whether an <see cref="AITool"/> was still created for this operation.</param>
    public sealed record OpenApiToolMappingIssue(string Path, string HttpMethod, string? OperationId, string Reason, bool ToolCreated);

    /// <summary>
    /// Result of generating <see cref="AITool"/> instances from the application's OpenAPI specification.
    /// </summary>
    public sealed class OpenApiAIToolsResult
    {
        public required IReadOnlyList<AITool> Tools { get; init; }

        public required IReadOnlyList<OpenApiToolMappingIssue> Issues { get; init; }
    }

    /// <summary>
    /// Builds and caches <see cref="AITool"/> instances generated from the app's own OpenAPI/Swagger document,
    /// ready to be exposed by an MCP server.
    /// </summary>
    public interface IOpenApiAIToolsProvider
    {
        Task<OpenApiAIToolsResult> GetToolsAsync(CancellationToken cancellationToken = default);
    }

    internal sealed class SwaggerOpenApiAIToolsProvider(
        ISwaggerProvider swaggerProvider,
        ILoggerFactory loggerFactory,
        string documentName) : IOpenApiAIToolsProvider
    {
        private readonly SemaphoreSlim _gate = new(1, 1);
        private OpenApiAIToolsResult? _cachedResult;

        public async Task<OpenApiAIToolsResult> GetToolsAsync(CancellationToken cancellationToken = default)
        {
            if (_cachedResult is not null)
                return _cachedResult;

            await _gate.WaitAsync(cancellationToken);
            try
            {
                if (_cachedResult is not null)
                    return _cachedResult;

                var document = swaggerProvider.GetSwagger(documentName);

                // Endpoints without an operationId still get an auto-generated name, but are flagged for visibility.
                var issues = DetectMissingOperationIds(document);

                var specJson = await document.SerializeAsJsonAsync(OpenApiSpecVersion.OpenApi3_0);
                using var specStream = new MemoryStream(Encoding.UTF8.GetBytes(specJson));

                // Wraps the library's logger so operations skipped due to real errors (invalid schema, etc.) are captured.
                var trackingLoggerFactory = new ToolMappingTrackingLoggerFactory(loggerFactory, issues);

                var options = new OpenApiFunctionExecutionParameters
                {
                    IgnoreNonCompliantErrors = true,
                    LoggerFactory = trackingLoggerFactory
                };

                var tools = await OpenApiToolFactory.GetToolsFromSpec(specStream, options, cancellationToken);

                _cachedResult = new OpenApiAIToolsResult { Tools = tools, Issues = issues };
                return _cachedResult;
            }
            finally
            {
                _gate.Release();
            }
        }

        private static List<OpenApiToolMappingIssue> DetectMissingOperationIds(OpenApiDocument document)
        {
            var issues = new List<OpenApiToolMappingIssue>();

            foreach (var (path, pathItem) in document.Paths)
            {
                if (pathItem.Operations is null)
                    continue;

                foreach (var (method, operation) in pathItem.Operations)
                {
                    if (string.IsNullOrWhiteSpace(operation.OperationId))
                    {
                        issues.Add(new OpenApiToolMappingIssue(
                            path,
                            method.Method,
                            operation.OperationId,
                            "Missing 'operationId' in the OpenAPI spec; a tool was still created using a name auto-generated from the HTTP method and path.",
                            ToolCreated: true));
                    }
                }
            }

            return issues;
        }
    }

    /// <summary>
    /// Logger that forwards every entry to the wrapped logger, while also turning the warning the
    /// package logs when an operation fails to render (e.g. invalid schema) into an <see cref="OpenApiToolMappingIssue"/>.
    /// </summary>
    internal sealed class ToolMappingTrackingLogger(ILogger inner, ICollection<OpenApiToolMappingIssue> sink) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => inner.BeginScope(state);

        public bool IsEnabled(LogLevel logLevel) => inner.IsEnabled(logLevel);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (logLevel == LogLevel.Warning && state is IEnumerable<KeyValuePair<string, object>> properties)
            {
                var operation = properties.FirstOrDefault(p => p.Key == "Operation").Value as RestApiOperation;
                if (operation is not null)
                {
                    var message = properties.FirstOrDefault(p => p.Key == "Message").Value as string ?? exception?.Message ?? "Unknown error.";
                    sink.Add(new OpenApiToolMappingIssue(operation.Path, operation.Method.Method, operation.Id, message, ToolCreated: false));
                }
            }

            inner.Log(logLevel, eventId, state, exception, formatter);
        }
    }

    internal sealed class ToolMappingTrackingLoggerFactory(ILoggerFactory inner, ICollection<OpenApiToolMappingIssue> sink) : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider) => inner.AddProvider(provider);

        public ILogger CreateLogger(string categoryName) => new ToolMappingTrackingLogger(inner.CreateLogger(categoryName), sink);

        public void Dispose()
        {
            // Owned by the caller; nothing to dispose here.
        }
    }

    public static class OpenApiAIToolsServiceCollectionExtensions
    {
        /// <summary>
        /// Registers an <see cref="IOpenApiAIToolsProvider"/> that generates <see cref="AITool"/> instances
        /// (one per OpenAPI operation) from the given Swagger document, so they can be exposed via an MCP server.
        /// </summary>
        public static IServiceCollection AddOpenApiAITools(this IServiceCollection services, string swaggerDocumentName = "v1")
        {
            services.AddSingleton<IOpenApiAIToolsProvider>(sp => new SwaggerOpenApiAIToolsProvider(
                sp.GetRequiredService<ISwaggerProvider>(),
                sp.GetRequiredService<ILoggerFactory>(),
                swaggerDocumentName));

            return services;
        }
    }
}
