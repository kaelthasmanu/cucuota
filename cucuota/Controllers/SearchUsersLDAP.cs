using Microsoft.AspNetCore.Mvc;

namespace cucuota.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchUsersLDAP:ControllerBase
{
    [HttpPost(Name = "SearchUsersLDAP")]
    public IActionResult Post(User request, [FromServices] LDAPUtils ldapUtils)
    {
        bool resp = ldapUtils.SearchUser(request.Username);
        if (resp)
        {
            return Ok("Users Exist.");
        }
        else
        {
            return BadRequest("User not Exist.");
        }
    }
}