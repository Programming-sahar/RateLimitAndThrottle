using System.Collections.Concurrent;

namespace ThrottleLiveSearch.Middleware;

public class ThrottleMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly ConcurrentDictionary<string, DateTime> _lastRequests = new();

    public ThrottleMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var key = $"{context.Connection.RemoteIpAddress}-{context.Request.Path}";
        if (_lastRequests.TryGetValue(key, out var last))
        {
            if ((DateTime.UtcNow - last) < TimeSpan.FromSeconds(3))
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsync("Too Many Requests");
                return;
            }
        }

        _lastRequests[key] = DateTime.UtcNow;
        await _next(context);
    }
}
