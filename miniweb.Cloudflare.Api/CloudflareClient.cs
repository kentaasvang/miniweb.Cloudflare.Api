using CloudFlare.Client.Api.Result;
using CloudFlare.Client.Api.Zones.DnsRecord;
using CloudFlare.Client.Enumerators;
using Microsoft.Extensions.Options;
using miniweb.Cloudflare.Api.Entities;
using CFClient = CloudFlare.Client.CloudFlareClient;

namespace miniweb.Cloudflare.Api;

public interface ICloudflareClient
{
  Task<CloudFlareResult<DnsRecord>> InsertDnsRecordAsync(string zoneId, DnsRecordEntity record);
  Task<bool>                        DeleteDnsRecordAsync(string zoneId, string recordId);
}

public class CloudflareClient : ICloudflareClient
{
  private readonly ILogger<CFClient> _logger;
  private readonly CFClient _client;

  public CloudflareClient(ILogger<CFClient> logger, IOptions<CloudflareAuthOptions> options)
  {
    _logger = logger;
    var options1 = options.Value;
    _client = new CFClient(options1.Email, options1.ApiKey);
  }

  public async Task<CloudFlareResult<DnsRecord>> InsertDnsRecordAsync(string zoneId, DnsRecordEntity record)
  {
    var result = await _client.Zones.DnsRecords.AddAsync(zoneId, new NewDnsRecord
    {
      Type = DnsRecordType.A,
      Name = record.Name,
      Content = record.Content,
      Proxied = false
    });

    return result;
  }

  public async Task<bool> DeleteDnsRecordAsync(string zoneId, string recordId)
  {
    var result = await _client.Zones.DnsRecords.DeleteAsync(zoneId, recordId);
    return result.Success;
  }
}