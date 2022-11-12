using Microsoft.AspNetCore.Mvc;

namespace miniweb.Cloudflare.Api.Controllers;

[Route("api/cloudflare")]
public class ApiController : Controller
{
  private readonly ILogger<ApiController> _logger;
  private readonly ICloudflareClient _client;

  public ApiController(ILogger<ApiController> logger, ICloudflareClient client)
  {
    _logger = logger;
    _client = client;
  }

  [HttpGet]
  public async Task<IActionResult> GetAllRecords() => Ok(await _client.GetAllDnsRecordsAsync());

  [HttpPost]
  public string AddRecord()
  {
    return "add";
  }

  [HttpPatch]
  public string UpdateRecord()
    => "update";

  [HttpDelete]
  public string DeleteRecord() => "delete";
}