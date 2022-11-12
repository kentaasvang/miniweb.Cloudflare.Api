namespace miniweb.Cloudflare.Api.Models;

public class Record
{
  public string? Name { get; set; }
  public RecordType Type { get; set; }
  public string? Content { get; set; }
}

public enum RecordType {
  A,
  AAAA,
  CNAME,
  MX,
  TXT,
  SRV
}