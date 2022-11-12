using Microsoft.EntityFrameworkCore;
using miniweb.Cloudflare.Api.Entities;

namespace miniweb.Cloudflare.Api.Data;

public class ApplicationDbContext : DbContext
{
  public DbSet<Zone>      Zones      => Set<Zone>();
  public DbSet<DnsRecord> DnsRecords => Set<DnsRecord>();

  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
  {
  }
}