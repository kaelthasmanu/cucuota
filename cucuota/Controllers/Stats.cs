using Microsoft.AspNetCore.Mvc;

namespace cucuota.Controllers;

[ApiController]
[Route("[controller]")]

public class Stats : ControllerBase
{
    private readonly ILogger<Cuota> _logger;

    public Stats(ILogger<Cuota> logger)
    {
        _logger = logger;
    }
    
    [HttpGet(Name = "GetStats")]
    // GET
    public string Get()
    {
        return GetStatsUsers.StatsUsers();
    }
}