namespace cucuota;

public class SquidData
{
    public string ProtocolVersion { get; set; }
    public int StatusCode { get; set; }
    public string Server { get; set; }
    public string MimeVersion { get; set; }
    public DateTime Date { get; set; }
    public string ContentType { get; set; }
    public DateTime Expires { get; set; }
    public DateTime LastModified { get; set; }
    public string CacheControl { get; set; }
    public List<string> CacheStatus { get; set; }
    public List<ConnectionInfo> Connections { get; set; }
}