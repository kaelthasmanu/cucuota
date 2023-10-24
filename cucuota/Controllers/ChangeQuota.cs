using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;

namespace cucuota.Controllers;

[ApiController]
[Route("[controller]")]
public class ChangeQuotaController : ControllerBase
{
    //[Authorize]
    [HttpPost(Name = "ChangeQuota")]
    public IActionResult Post(UserData request, [FromServices] ChangeCantQuota changeCantQuota)
    {
        bool resp = changeCantQuota.Change(request.Username, request.Gigabytes);
        if (resp)
        {
            return Ok("Quota updated successfully.");
        }
        else
        {
            return BadRequest("Failed to update the quota.");
        }
    }
}