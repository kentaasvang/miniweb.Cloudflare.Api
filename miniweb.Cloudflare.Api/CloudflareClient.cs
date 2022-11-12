using CloudFlare.Client.Api.Zones.DnsRecord;
using Microsoft.Extensions.Options;
using CFClient = CloudFlare.Client.CloudFlareClient;

namespace miniweb.Cloudflare.Api;


public interface ICloudflareClient
{
  Task<IEnumerable<DnsRecord>> GetAllDnsRecordsAsync();
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

  public async Task<IEnumerable<DnsRecord>> GetAllDnsRecordsAsync()
  {
    var dnsRecords = new List<DnsRecord>();
    var zones = await _client.Zones.GetAsync();

    foreach (var zone in zones.Result)
    {
      var dns = await _client.Zones.DnsRecords.GetAsync(zone.Id);

      if (dns.Success)
        dnsRecords.AddRange(dns.Result);
      else
        _logger.LogError($"Failed to get dns-records with message: {dns.Errors}");
    }

    return dnsRecords;
  }
}