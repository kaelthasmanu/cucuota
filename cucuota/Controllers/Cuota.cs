using Microsoft.AspNetCore.Mvc;

namespace cucuota.Controllers;

[ApiController]
[Route("[controller]")]
public class Cuota : ControllerBase
{
    
    private readonly ILogger<Cuota> _logger;

    public Cuota(ILogger<Cuota> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetCuota")]
    public string Get([FromServices] Database database)
    {
        return database.GetAllUserDataAsJson();
    }
}

[ApiController]
[Route("[controller]")]
public class SiteMultiplierQuota : ControllerBase
{
    [HttpPost(Name = "SiteMultiplierQuota")]
    public bool SetMultiplier([FromServices] Database database, string site, int multiplier)
    {
        try
        {
            database.AddSiteQuota(site, multiplier);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ha ocurrido un error en el controlador: {e}");
            return false;
        }
    }
}