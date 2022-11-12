using CloudFlare.Client.Api.Zones.DnsRecord;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using miniweb.Cloudflare.Api.Data;
using CFClient = CloudFlare.Client.CloudFlareClient;

namespace miniweb.Cloudflare.Api;

public interface ICloudflareClient
{
  Task<IEnumerable<DnsRecord>> GetAllDnsRecordsAsync();
}

public class CloudflareClient : ICloudflareClient
{
  private readonly ILogger<CFClient> _logger;
  private readonly ApplicationDbContext _dbContext;
  private readonly CFClient _client;

  public CloudflareClient(ILogger<CFClient> logger, IOptions<CloudflareAuthOptions> options,
    ApplicationDbContext dbContext)
  {
    _logger = logger;
    _dbContext = dbContext;
    var options1 = options.Value;
    _client = new CFClient(options1.Email, options1.ApiKey);
  }

  public async Task<IEnumerable<DnsRecord>> GetAllDnsRecordsAsync()
  {
    var dnsRecords = new List<DnsRecord>();
    var zoneIds = await (from zone in _dbContext.Zones
                         select zone.Id).ToListAsync();

    foreach (var zoneId in zoneIds)
    {
      var dns = await _client.Zones.DnsRecords.GetAsync(zoneId);

      if (dns.Success)
        dnsRecords.AddRange(dns.Result);
      else
        _logger.LogError($"Failed to get dns-records with message: {dns.Errors}");
    }

    return dnsRecords;
  }
}