using Microsoft.AspNetCore.Mvc;

namespace miniweb.Cloudflare.Api.Controllers;

[Route("api")]
public class ApiController : Controller
{
  private readonly ILogger<ApiController> _logger;

  public ApiController(ILogger<ApiController> logger)
  {
    _logger = logger;
  }

  [HttpGet]
  public string Index()
  {
    return "hello";
  }
}