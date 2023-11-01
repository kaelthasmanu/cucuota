using Microsoft.AspNetCore.Mvc;

namespace cucuota.Controllers;

[ApiController]
[Route("[controller]")]

public class CreateUsersAdmin:ControllerBase
{
    [HttpPost(Name = "CreateUsersAdmin")]
    public IActionResult Post(string username, bool admin)
    {
        if (admin)
        {
            if (Database.CreateAdmin(username))
            {
                return Ok("Is Valid.");
            }
        }
        return BadRequest("Not Valid");
    }    
}

[ApiController]
[Route("[controller]")]
public class DeleteUsersAdmin:ControllerBase
{
    [HttpPost(Name = "DeleteUsersAdmin")]
    public IActionResult Post(string request)
    {
        bool isValid = Database.IsValidEmail(request);
        if (isValid)
        {
            return Ok("Is Valid.");
        }
        return BadRequest("Not Valid");
    }    
}