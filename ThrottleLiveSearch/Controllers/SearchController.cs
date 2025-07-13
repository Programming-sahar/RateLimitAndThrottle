using ThrottleLiveSearch.Services;

namespace ThrottleLiveSearch.Controllers;


using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly IStaticSearchService _searchService;
    private static readonly ConcurrentDictionary<string, DateTime> _lastRequestPerIp = new();

    public SearchController(IStaticSearchService searchService)
    {
        _searchService = searchService;
    }

    [HttpGet("query")]
    public IActionResult Search([FromQuery] string q)
    {
        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        // بررسی محدودیت زمانی
        if (_lastRequestPerIp.TryGetValue(clientIp, out var lastTime))
        {
            var elapsed = DateTime.UtcNow - lastTime;
            if (elapsed < TimeSpan.FromSeconds(3))
            {
                return StatusCode(429, "زیادی سریع تایپ کردی! لطفاً چند ثانیه صبر کن 🙂");
            }
        }

        _lastRequestPerIp[clientIp] = DateTime.UtcNow;

        var list = _searchService.GetSearchList();
        var result = list
            .Where(x => x.Contains(q, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(result);
    }


    [HttpGet("SearchByMiddleware")]
    public IActionResult SearchByMiddleware([FromQuery] string q)
    {
        var list = _searchService.GetSearchList();
        var result = list
            .Where(x => x.Contains(q, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(result);
    }
}
