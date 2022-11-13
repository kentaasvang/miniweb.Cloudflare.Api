using CloudFlare.Client.Api.Zones.DnsRecord;
using Microsoft.EntityFrameworkCore;
using miniweb.Cloudflare.Api.Data;
using miniweb.Cloudflare.Api.Entities;

namespace miniweb.Cloudflare.Api.Services;

public interface IDnsRecordService
{
  Task<DnsRecordServiceResult<List<DnsRecordEntity>>> GetAllAsync();
  Task<DnsRecordServiceResult<DnsRecordEntity>>       CreateAsync(DnsRecordEntity dnsRecordEntity);
  Task<DnsRecordServiceResult<DnsRecordEntity>>       UpdateAsync(DnsRecordEntity dnsRecordEntity);
  Task<DnsRecordServiceResult<DnsRecordEntity>>       DeleteAsync(DnsRecordEntity dnsRecordEntity);
  Task<DnsRecordServiceResult<DnsRecordEntity>>       GetByIdAsync(string id);
}

public class DnsRecordService : IDnsRecordService
{
  private readonly ApplicationDbContext _dbContext;
  private readonly ICloudflareClient _client;

  public DnsRecordService(ApplicationDbContext dbContext, ICloudflareClient client)
  {
    _dbContext = dbContext;
    _client = client;
  }

  public async Task<DnsRecordServiceResult<List<DnsRecordEntity>>> GetAllAsync()
  {
    var dnsRecords = await (from records in _dbContext.DnsRecords
                            select records).ToListAsync();

    return new DnsRecordServiceResult<List<DnsRecordEntity>>
    {
      Succeeded = true,
      Result = dnsRecords
    };
  }

  public async Task<DnsRecordServiceResult<DnsRecordEntity>> CreateAsync(DnsRecordEntity dnsRecordEntity)
  {
    var zoneId = await _dbContext.Zones.Select(zone => zone.Id).FirstAsync();
    var result = await _client.InsertDnsRecordAsync(zoneId, dnsRecordEntity);

    if (!result)
      return new DnsRecordServiceResult<DnsRecordEntity>
      {
        Succeeded = false,
        ErrorMessage = "Could not insert new DnsRecord"
      };

    var entry = await _dbContext.DnsRecords.AddAsync(dnsRecordEntity);
    await _dbContext.SaveChangesAsync();

    return new DnsRecordServiceResult<DnsRecordEntity>
    {
      Succeeded = true,
      Result = entry.Entity
    };
  }

  public async Task<DnsRecordServiceResult<DnsRecordEntity>> UpdateAsync(DnsRecordEntity dnsRecordEntity)
  {
    var entry = _dbContext.DnsRecords.Update(dnsRecordEntity);
    await _dbContext.SaveChangesAsync();

    return new DnsRecordServiceResult<DnsRecordEntity>
    {
      Succeeded = true,
      Result = entry.Entity
    };
  }

  public async Task<DnsRecordServiceResult<DnsRecordEntity>> DeleteAsync(DnsRecordEntity dnsRecordEntity)
  {
    var entry = _dbContext.DnsRecords.Remove(dnsRecordEntity);
    await _dbContext.SaveChangesAsync();

    return new DnsRecordServiceResult<DnsRecordEntity>
    {
      Succeeded = true,
      Result = entry.Entity
    };
  }

  public async Task<DnsRecordServiceResult<DnsRecordEntity>> GetByIdAsync(string id)
  {
    var entry = await (from record in _dbContext.DnsRecords
                where record.Id == id
                select record).ToListAsync();

    return entry.Any()
      ? new DnsRecordServiceResult<DnsRecordEntity>
      {
        Succeeded = true,
        Result = entry.First()
      }
      : new DnsRecordServiceResult<DnsRecordEntity>
      {
        Succeeded = false,
        ErrorMessage = $"DnsRecord with id: {id} was not found"
      };
  }
}

public class DnsRecordServiceResult<T> : IServiceResult<T>
{
  public bool    Succeeded    { get; set; }
  public string? ErrorMessage { get; set; }
  public T?      Result       { get; set; }
}