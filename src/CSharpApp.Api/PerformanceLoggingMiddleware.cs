using System.Diagnostics;

namespace CSharpApp.Api;

public class PerformanceLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<PerformanceLoggingMiddleware> _logger;

    public PerformanceLoggingMiddleware(RequestDelegate next, ILogger<PerformanceLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        await _next(context);

        stopwatch.Stop();

        var elapsedMs = stopwatch.ElapsedMilliseconds;

        _logger.LogInformation(
            "Request {method} {url} executed in {duration} ms",
            context.Request.Method,
            context.Request.Path,
            elapsedMs
        );
    }
}