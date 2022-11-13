using Microsoft.AspNetCore.Mvc;
using miniweb.Cloudflare.Api.Entities;
using miniweb.Cloudflare.Api.Services;

namespace miniweb.Cloudflare.Api.Controllers;

[Route("api/cloudflare")]
public class ApiController : Controller
{
  private readonly IDnsRecordService _dnsRecordService;
  private readonly ICloudflareClient _client;

  public ApiController(IDnsRecordService dnsRecordService, ICloudflareClient client)
  {
    _dnsRecordService = dnsRecordService;
    _client = client;
  }

  [HttpGet]
  public async Task<IActionResult> GetAllAsync()
  {
    var result = await _dnsRecordService.GetAllAsync();
    return result.Succeeded
      ? Ok(result.Result)
      : BadRequest(result.ErrorMessage);
  }
  
  [HttpGet("{id}")]
  public async Task<IActionResult> GetRecordAsync(string id)
  {
    var result = await _dnsRecordService.GetByIdAsync(id);
    return result.Succeeded
      ? Ok(result.Result)
      : NotFound();
  }

  [HttpPost]
  public async Task<IActionResult> AddRecord([FromBody] DnsRecordEntity dnsRecord)
  {
    var result = await _dnsRecordService.CreateAsync(dnsRecord);
    
    return result.Succeeded
      ? Created(nameof(GetRecordAsync), result.Result)
      : BadRequest(result.ErrorMessage);
  }

  [HttpPatch]
  public async Task<IActionResult> UpdateRecord([FromBody] DnsRecordEntity dnsRecord)
  {
    var result = await _dnsRecordService.UpdateAsync(dnsRecord);
    return result.Succeeded
      ? Created(nameof(GetRecordAsync), result.Result)
      : BadRequest(result.ErrorMessage);
  }

  [HttpDelete]
  public async Task<IActionResult> DeleteRecord([FromBody] DnsRecordEntity dnsRecord)
  {
    var result = await _dnsRecordService.DeleteAsync(dnsRecord);
    return result.Succeeded
      ? Ok()
      : BadRequest(result.ErrorMessage);
  }
}