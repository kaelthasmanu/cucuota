using Microsoft.AspNetCore.Mvc;

namespace cucuota.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthUserLDAP:ControllerBase
{
    [HttpPost(Name = "AuthUserLDAP")]
    public IActionResult Post(UserLDAP request, [FromServices] LDAPUtils ldapUtils)
    {
        bool resp = ldapUtils.AuthenticateUser(request.Username, request.Password);
        if (resp)
        {
            return Ok("Users Auth OK.");
        }
        else
        {
            return BadRequest("Credentials invalid or user not exist.");
        }
    }
}