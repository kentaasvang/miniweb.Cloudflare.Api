namespace miniweb.Cloudflare.Api;

public static class Extensions
{
  public static IServiceCollection AddCloudflareClient(this IServiceCollection serviceCollection, IConfiguration config)
  {
    serviceCollection.Configure<CloudflareAuthOptions>(
      config.GetSection("Cloudflare"));
    
    serviceCollection.AddTransient<ICloudflareClient, CloudflareClient>();
    return serviceCollection;
  }
}