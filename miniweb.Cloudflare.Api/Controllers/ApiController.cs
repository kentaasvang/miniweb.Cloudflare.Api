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
  public async Task<IActionResult> GetAllRecords()
  {
    _logger.LogInformation("Getting all DNS-records");
    return Ok(await _client.GetAllDnsRecordsAsync());
  }

  [HttpPost]
  public string AddRecord() => throw new NotImplementedException();

  [HttpPatch]
  public string UpdateRecord() => throw new NotImplementedException();

  [HttpDelete]
  public string DeleteRecord() => throw new NotImplementedException();
}