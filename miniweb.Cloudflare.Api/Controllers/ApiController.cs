using Microsoft.AspNetCore.Mvc;
using miniweb.Cloudflare.Api.Dtos;
using miniweb.Cloudflare.Api.Services;

namespace miniweb.Cloudflare.Api.Controllers;

[Route("api/cloudflare")]
public class ApiController : Controller
{
  private readonly IDnsRecordService _dnsRecordService;

  public ApiController(IDnsRecordService dnsRecordService)
  {
    _dnsRecordService = dnsRecordService;
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
  public async Task<IActionResult> AddRecord([FromBody] DnsRecordDto dnsRecord)
  {
    var result = await _dnsRecordService.CreateAsync(dnsRecord);
    
    return result.Succeeded
      ? Created(nameof(GetRecordAsync), result.Result)
      : BadRequest(result.ErrorMessage);
  }

  [HttpPatch]
  public async Task<IActionResult> UpdateRecord([FromBody] DnsRecordDto dnsRecord)
  {
    var result = await _dnsRecordService.UpdateAsync(dnsRecord);
    return result.Succeeded
      ? Created(nameof(GetRecordAsync), result.Result)
      : BadRequest(result.ErrorMessage);
  }

  [HttpDelete]
  public async Task<IActionResult> DeleteRecord([FromBody] DnsRecordDto dnsRecord)
  {
    var result = await _dnsRecordService.DeleteAsync(dnsRecord);
    return result.Succeeded
      ? Ok()
      : BadRequest(result.ErrorMessage);
  }
}