using Microsoft.AspNetCore.Mvc;

namespace RateLimiterByPackage.Controllers;


[Route("api/[controller]")]
[ApiController]
public class EchoController : ControllerBase
{
    [HttpGet]
    public IActionResult Get(string text)
    {
        return Ok($"Echo: {text}");
    }
}