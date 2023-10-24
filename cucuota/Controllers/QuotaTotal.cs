using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace cucuota.Controllers;

[ApiController]
[Route("[controller]")]
public class QuotaTotal : ControllerBase
{
    private readonly ILogger<Cuota> _logger;

    public QuotaTotal(ILogger<Cuota> logger)
    {
        _logger = logger;
    }
    static List<Dictionary<string, object>> GetQuotaTotal()
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(
            "appsettings.json",
            optional: false,
            reloadOnChange: true
        );

        IConfiguration config = builder.Build();
        var list = Parsing.ParseUserData(config.GetSection("WorkingFiles").GetSection("QuoteFile").Value);
        var listdata = new List<Dictionary<string, object>>();
        foreach (var datauser in list)
        {
            var userData = new Dictionary<string, object>
            {
                {"name", datauser.Username},
                {"totalQuota", datauser.Gigabytes},
                
            };
            listdata.Add(userData);
            
        }
        return listdata;
    }

    [HttpGet(Name = "GetQuotaTotal")]
    public string SendData()
    {
        return JsonConvert.SerializeObject(GetQuotaTotal());
    }

}