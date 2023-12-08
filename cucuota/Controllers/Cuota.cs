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