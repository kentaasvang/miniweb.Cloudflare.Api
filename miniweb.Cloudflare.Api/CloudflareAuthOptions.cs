namespace miniweb.Cloudflare.Api;

public class CloudflareAuthOptions
{
  public const string CloudflareAuth = "CloudflareAuth";
  public string Email  { get; set; } = string.Empty;
  public string ApiKey { get; set; } = string.Empty;
}