using CloudFlare.Client.Api.Zones.DnsRecord;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miniweb.Cloudflare.Api.Data;
using miniweb.Cloudflare.Api.Dtos;
using miniweb.Cloudflare.Api.Entities;

namespace miniweb.Cloudflare.Api.Services;

public interface IDnsRecordService
{
  Task<DnsRecordServiceResult<List<DnsRecordDto>>> GetAllAsync();
  Task<DnsRecordServiceResult<DnsRecordDto>>       CreateAsync(DnsRecordDto dnsRecord);
  Task<DnsRecordServiceResult<DnsRecordDto>>       UpdateAsync(DnsRecordDto dnsRecord);
  Task<DnsRecordServiceResult<DnsRecordDto>>       DeleteAsync(DnsRecordDto dnsRecordEntity);
  Task<DnsRecordServiceResult<DnsRecordDto>>       GetByIdAsync(string id);
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

  public async Task<DnsRecordServiceResult<List<DnsRecordDto>>> GetAllAsync()
  {
    var dnsRecords = await (from records in _dbContext.DnsRecords
                            select records).ToListAsync();

    var dtos = ToDto(dnsRecords);

    return new DnsRecordServiceResult<List<DnsRecordDto>>
    {
      Succeeded = true,
      Result = dtos
    };
  }

  public async Task<DnsRecordServiceResult<DnsRecordDto>> CreateAsync(DnsRecordDto dnsRecord)
  {
    var zone = await _dbContext.Zones.Select(zone => zone).FirstAsync();

    var entity = new DnsRecordEntity()
    {
      ZoneEntity = zone,
      Name = dnsRecord.Name,
      Content = dnsRecord.Content
    };

    var result = await _client.InsertDnsRecordAsync(zone.Id, entity);

    if (!result.Success)
      return new DnsRecordServiceResult<DnsRecordDto>
      {
        Succeeded = false,
        ErrorMessage = "Could not insert new DnsRecord"
      };

    entity.Id = result.Result.Id;

    var entry = await _dbContext.DnsRecords.AddAsync(entity);
    await _dbContext.SaveChangesAsync();

    return new DnsRecordServiceResult<DnsRecordDto>
    {
      Succeeded = true,
      Result = ToDto(entry.Entity)
    };
  }

  public async Task<DnsRecordServiceResult<DnsRecordDto>> UpdateAsync(DnsRecordDto dnsRecord)
  {
    var entity = await (from record in _dbContext.DnsRecords
                        where record.Name == dnsRecord.Name
                        select record).ToListAsync();

    if (!entity.Any())
      return new DnsRecordServiceResult<DnsRecordDto>
      {
        Succeeded = true,
        ErrorMessage = $"Could not find dns-record with name {dnsRecord.Name}"
      };

    var entry = _dbContext.DnsRecords.Update(entity.First());
    await _dbContext.SaveChangesAsync();

    return new DnsRecordServiceResult<DnsRecordDto>
    {
      Succeeded = true,
      Result = ToDto(entry.Entity)
    };
  }

  public async Task<DnsRecordServiceResult<DnsRecordDto>> DeleteAsync(DnsRecordDto record)
  {
    var zoneId = await _dbContext.Zones.Select(zone => zone.Id).FirstAsync();
    var entity = await (from dnsRecord in _dbContext.DnsRecords
                        where dnsRecord.Name == record.Name
                        select dnsRecord).ToListAsync();

    if (!entity.Any())
      return new DnsRecordServiceResult<DnsRecordDto>
      {
        Succeeded = false,
        ErrorMessage = $"Did not successfully delete record from Cloudflare with name: {record.Name}"
      };

    var result = await _client.DeleteDnsRecordAsync(zoneId, entity.First().Id);
    
    if (!result)
      return new DnsRecordServiceResult<DnsRecordDto>
      {
        Succeeded = false,
        ErrorMessage = $"Did not successfully delete record from database with name: {record.Name}"
      };

    var entry = _dbContext.DnsRecords.Remove(entity.First());
    await _dbContext.SaveChangesAsync();

    return new DnsRecordServiceResult<DnsRecordDto>
    {
      Succeeded = true,
      Result = ToDto(entry.Entity)
    };
  }

  public async Task<DnsRecordServiceResult<DnsRecordDto>> GetByIdAsync(string id)
  {
    var entry = await (from record in _dbContext.DnsRecords
                       where record.Id == id
                       select record).ToListAsync();

    if (!entry.Any())
      return new DnsRecordServiceResult<DnsRecordDto>
      {
        Succeeded = false,
        ErrorMessage = $"DnsRecord with id: {id} was not found"
      };

    var dto = ToDto(entry.First());

    return new DnsRecordServiceResult<DnsRecordDto>
    {
      Succeeded = true,
      Result = dto
    };
  }

#warning replace with AutoMapper
  private static DnsRecordDto ToDto(DnsRecordEntity entity)
    => new()
    {
      Name = entity.Name,
      Content = entity.Content
    };

#warning replace with AutoMapper
  private static List<DnsRecordDto> ToDto(List<DnsRecordEntity> entities)
    => (from entity in entities select ToDto(entity)).ToList();
}

public class DnsRecordServiceResult<T> : IServiceResult<T>
{
  public bool    Succeeded    { get; set; }
  public string? ErrorMessage { get; set; }
  public T?      Result       { get; set; }
}