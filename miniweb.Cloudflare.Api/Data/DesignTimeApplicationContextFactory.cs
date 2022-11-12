using Microsoft.EntityFrameworkCore;

namespace miniweb.Cloudflare.Api.Data;

/// <summary>
/// This class is only used by `dotnet ef`-CLI tool when running migrations
/// </summary>
public class DesignTimeApplicationContextFactory
{
  public ApplicationDbContext CreateDbContext(string[] args)
  {
    var optionsBuilder   = new DbContextOptionsBuilder<ApplicationDbContext>();
    var connectionString = GetConnectionString();
    optionsBuilder.UseSqlite(connectionString);

    return new ApplicationDbContext(optionsBuilder.Options);
  }

  private static string GetConnectionString()
  {
    // TODO: hardcoding is bad. 
    return "DataSource=app.db;Cache=Shared";
  } 
}