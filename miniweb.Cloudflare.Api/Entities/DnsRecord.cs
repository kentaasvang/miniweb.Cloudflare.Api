using CloudFlare.Client.Enumerators;

namespace miniweb.Cloudflare.Api.Entities;

public class DnsRecord
{
  public string        Id         { get; set; } = string.Empty;
  public string        Name       { get; set; } = string.Empty;
  public DnsRecordType RecordType { get; set; }
  public string        Content    { get; set; } = string.Empty;
  public Zone          Zone       { get; set; }
}