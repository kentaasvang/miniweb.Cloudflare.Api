using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace miniweb.Cloudflare.Api.Entities;

[Table("zones")]
public class ZoneEntity
{
  [Key] public string                       Id         { get; set; } = string.Empty;
  public       string                       Name       { get; set; } = string.Empty;
  private      IEnumerable<DnsRecordEntity> DnsRecords { get; set; } = new List<DnsRecordEntity>();
}