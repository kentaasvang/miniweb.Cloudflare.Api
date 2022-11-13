using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CloudFlare.Client.Enumerators;

namespace miniweb.Cloudflare.Api.Entities;

[Table("dns_records")]
public class DnsRecordEntity
{
  [Key] public string        Id         { get; set; } = string.Empty;
  public       string        Name       { get; set; } = string.Empty;
  public       DnsRecordType RecordType { get; set; }
  public       string        Content    { get; set; } = string.Empty;
  public       ZoneEntity?   ZoneEntity { get; set; }
}