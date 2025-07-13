using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace RateLimiterByNetCore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DisplayController : ControllerBase
{
    [HttpGet]
    [EnableRateLimiting("fixed")] // 👉 استفاده از همون policy با نام "fixed"
    public IActionResult Get([FromQuery] string text)
    {
        return Ok($"Display: {text}");
        // when requests be more than 1 in 1 min, response is: HTTP 429 Too Many Requests 
    }
}


