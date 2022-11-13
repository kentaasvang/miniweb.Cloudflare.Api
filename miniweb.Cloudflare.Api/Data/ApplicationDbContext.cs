using Microsoft.EntityFrameworkCore;
using miniweb.Cloudflare.Api.Entities;

namespace miniweb.Cloudflare.Api.Data;

public class ApplicationDbContext : DbContext
{
  public DbSet<ZoneEntity>      Zones      => Set<ZoneEntity>();
  public DbSet<DnsRecordEntity> DnsRecords => Set<DnsRecordEntity>();

  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
  {
  }
}