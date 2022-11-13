namespace miniweb.Cloudflare.Api.Services;

public interface IServiceResult<T>
{
  public bool    Succeeded    { get; set; }
  public string? ErrorMessage { get; set; }
  public T?      Result       { get; set; } 
}