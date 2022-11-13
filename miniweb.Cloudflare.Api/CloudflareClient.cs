using CloudFlare.Client.Api.Zones.DnsRecord;
using CloudFlare.Client.Enumerators;
using Microsoft.Extensions.Options;
using miniweb.Cloudflare.Api.Entities;
using CFClient = CloudFlare.Client.CloudFlareClient;

namespace miniweb.Cloudflare.Api;

public interface ICloudflareClient
{
  Task<bool>                   InsertDnsRecordAsync(string zoneId, DnsRecordEntity record);
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

  public async Task<bool> InsertDnsRecordAsync(string zoneId, DnsRecordEntity record)
  {
    var result = await _client.Zones.DnsRecords.AddAsync(zoneId, new NewDnsRecord
    {
      Type = DnsRecordType.A,
      Name = record.Name,
      Content = record.Content,
      Proxied = false
    });

    return result.Success;
  }
}