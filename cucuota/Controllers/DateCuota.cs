using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace cucuota.Controllers;

[ApiController]
[Route("[controller]")]
public class DateCuota : ControllerBase
{
    [HttpPost]
    public IActionResult Post(string requestName, DateTime requestTime, int requestPercent ,[FromServices] Database database)
    {
        var result = database.AddTimeCuotaPref(requestName , requestTime , requestPercent);
        if (result == true)
        {
            return Ok("Add TimeCuota successfully.");
        }
        else
        {
            return BadRequest("Failed to add TimeCuota.");
        }
    }
    
    [HttpGet]
    public IActionResult Get([FromServices] Database database)
    {
        try
        {
            var result = database.GetAllTimeCuotaPrefs();

            if (result != null && result.Any())
            {
                var data = new List<Dictionary<string, object>>();

                foreach (var timeCuotaPref in result)
                {
                    var itemData = new Dictionary<string, object>();

                    foreach (var property in typeof(TimeCuotaPref).GetProperties())
                    {
                        itemData.Add(property.Name, property.GetValue(timeCuotaPref));
                    }

                    data.Add(itemData);
                }

                var responseData = new Dictionary<string, object>
                {
                    { "message", "Success" },
                    { "query", data },
                };

                return Ok(JsonConvert.SerializeObject(responseData));
            }
            else
            {
                return BadRequest("No TimeCuotaPrefs found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return StatusCode(500, "Internal Server Error");
        }
    }

}