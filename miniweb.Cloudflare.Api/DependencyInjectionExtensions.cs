using Microsoft.EntityFrameworkCore;
using miniweb.Cloudflare.Api.Data;
using miniweb.Cloudflare.Api.Services;

namespace miniweb.Cloudflare.Api;

public static class DependencyInjectionExtensions
{
  public static IServiceCollection AddCloudflareClient(this IServiceCollection serviceCollection, IConfiguration config)
  {
    serviceCollection.Configure<CloudflareAuthOptions>(
      config.GetSection("Cloudflare"));
    
    serviceCollection.AddTransient<ICloudflareClient, CloudflareClient>();
    return serviceCollection;
  }

  public static IServiceCollection AddDatabase(this IServiceCollection serviceCollection, IConfiguration config)
  {
    var connectionString = config.GetSection("Database:ConnectionString");
    
    serviceCollection.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlite(connectionString.Value));

    return serviceCollection;
  }
  
  public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
  {
    serviceCollection.AddTransient<IDnsRecordService, DnsRecordService>();
    return serviceCollection;
  }
  
  
}