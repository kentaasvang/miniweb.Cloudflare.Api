namespace miniweb.Cloudflare.Api.Entities;

public class Zone
{
  public  string                 Id         { get; set; } = string.Empty;
  public  string                 Name       { get; set; } = string.Empty;
  private IEnumerable<DnsRecord> DnsRecords { get; set; } = new List<DnsRecord>();
}