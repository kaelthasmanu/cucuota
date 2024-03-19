using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace cucuota;

public class ConnectionInfo
{
    public string Connection { get; set; }
    public string FdInfo { get; set; }
    public string FdDesc { get; set; }
    public string InInfo { get; set; }
    public string Remote { get; set; }
    public string Local { get; set; }
    public string NRequests { get; set; }
    public string Uri { get; set; }
    public string LogType { get; set; }
    public string OutOffset { get; set; }
    public string OutSize { get; set; }
    public string RequestSize { get; set; }
    public string Entry { get; set; }
    public string Start { get; set; }
    public string Username { get; set; }
    public string DelayPool { get; set; }
}

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

public class GetStatsUsers
{
    public static string StatsUsers()
    {
        /*ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "squidclient",          
            Arguments = "-h 127.0.0.1 mgr:active_requests",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        
        Process process = new Process { StartInfo = psi };
        
        process.Start();
        
        string output = process.StandardOutput.ReadToEnd();
        
        process.WaitForExit();*/
        string output =
            "HTTP/1.1 200 OK\nServer: squid\nMime-Version: 1.0\nDate: Thu, 21 Sep 2023 21:11:52 GMT\nContent-Type: text/plain;charset=utf-8\nExpires: Thu, 21 Sep 2023 21:11:52 GMT\nLast-Modified: Thu, 21 Sep 2023 21:11:52 GMT\nCache-Control: no-cache, no-store\nCache-Status: anonymous;detail=mismatch\nCache-Status: anonymous;detail=no-cache\nConnection: close\n\nConnection: 0x55b5c1197ad8\n\tFD 22, read 155, wrote 0\n\tFD desc: http://anonymous:3128/squid-internal-mgr/active_requests\n\tin: buf 0x55b5c198e0e0, used 0, free 3940\n\tremote: 127.0.0.1:44434\n\tlocal: 127.0.0.1:3128\n\tnrequests: 1\nuri http://anonymous:3128/squid-internal-mgr/active_requests\nlogType TCP_MISS\nout.offset 0, out.size 0\nreq_sz 155\nentry 0x55b5c18d9990/E1050000000000002CE3000001000000\nstart 1695330712.855978 (0.000175 seconds ago)\nusername -\ndelay_pool 0\n\nConnection: 0x55b5c195a8c8\n\tFD 12, read 176, wrote 0\n\tFD desc: http://127.0.0.1:3128/squid-internal-mgr/active_requests\n\tin: buf 0x55b5c18214e0, used 0, free 3919\n\tremote: 127.0.0.1:44428\n\tlocal: 127.0.0.1:3128\n\tnrequests: 1\nuri http://127.0.0.1:3128/squid-internal-mgr/active_requests\nlogType TCP_MISS\nout.offset 0, out.size 0\nreq_sz 176\nentry 0x55b5c1aed970/E0050000000000002CE3000001000000\nstart 1695330712.855245 (0.000908 seconds ago)\nusername -\ndelay_pool 0";;
        
        string protocolPattern = @"HTTP/(\d\.\d) (\d{3}) (\w+)";
        string connectionPattern = @"Connection: (\S+)\s+(FD \d+[^:]+:\d+)\s+FD desc: (\S+)\s+in: buf (\S+), used \d+, free (\d+)\s+remote: (\S+)\s+local: (\S+)\s+nrequests: (\d+)\s+uri (\S+)\s+logType (\w+)\s+out.offset (\d+), out.size (\d+)\s+req_sz (\d+)\s+entry (\S+)\s+start (\S+) \((\d+\.\d+) seconds ago\)\s+username (\S+)\s+delay_pool (\d+)";
        string cacheStatusPattern = @"Cache-Status: (\S+);\s*detail=([\w-]+)";

        // Use regex to match and extract data.
        string[] lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Match protocolMatch = Regex.Match(lines[0], protocolPattern);
        MatchCollection connectionMatches = Regex.Matches(output, connectionPattern);
        MatchCollection cacheStatusMatches = Regex.Matches(output, cacheStatusPattern);
        
        SquidData squidData = new SquidData
        {
            ProtocolVersion = protocolMatch.Groups[1].Value,
            StatusCode = int.Parse(protocolMatch.Groups[2].Value),
            Server = protocolMatch.Groups[3].Value,
            // Parse other properties here...
            CacheStatus = new List<string>(),
            Connections = new List<ConnectionInfo>()
        };

        foreach (Match cacheStatusMatch in cacheStatusMatches)
        {
            string status = cacheStatusMatch.Groups[1].Value;
            string detail = cacheStatusMatch.Groups[2].Value;
            squidData.CacheStatus.Add($"{status}; detail={detail}");
        }

        foreach (Match connectionMatch in connectionMatches)
        {
            ConnectionInfo connectionInfo = new ConnectionInfo
            {
                Connection = connectionMatch.Groups[1].Value,
                FdInfo = connectionMatch.Groups[2].Value,
                FdDesc = connectionMatch.Groups[3].Value,
                InInfo = connectionMatch.Groups[4].Value,
                Remote = connectionMatch.Groups[5].Value,
                Local = connectionMatch.Groups[6].Value, 
                NRequests = connectionMatch.Groups[7].Value,
                Uri = connectionMatch.Groups[8].Value,
                LogType = connectionMatch.Groups[9].Value,
                OutOffset = connectionMatch.Groups[10].Value,
                OutSize = connectionMatch.Groups[11].Value,
                RequestSize = connectionMatch.Groups[12].Value,
                Entry = connectionMatch.Groups[13].Value,
                Start = connectionMatch.Groups[14].Value,
                Username = connectionMatch.Groups[15].Value,
                DelayPool = connectionMatch.Groups[16].Value  // Use the correct capture group index
                // Parse other properties here...
            };
            squidData.Connections.Add(connectionInfo);
        }
        
        string jsonData = JsonConvert.SerializeObject(squidData, Formatting.Indented);
        
        return jsonData;
    }
}