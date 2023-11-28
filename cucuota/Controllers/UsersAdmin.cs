using Microsoft.AspNetCore.Mvc;

namespace cucuota.Controllers;

[ApiController]
[Route("[controller]")]

public class CreateUsersAdmin:ControllerBase
{
    //[Authorize]
    [HttpPost(Name = "CreateUsersAdmin")]
    public IActionResult Post(string username, bool admin)
    {
        if (admin)
        {
            if (Database.CreateAdmin(username))
            {
                return Ok("User Create.");
            }
        }
        return BadRequest("Not Valid");
    }    
}

[ApiController]
[Route("[controller]")]
public class DeleteUsersAdmin:ControllerBase
{
    //[Authorize]
    [HttpPost(Name = "DeleteUsersAdmin")]
    public IActionResult Post(string username)
    {
        if (Database.DeleteAdmin(username))
        {
            return Ok("User Deleted.");    
        }
        return BadRequest("Not Valid");
    }    
}

[ApiController]
[Route("[controller]")]
public class CheckAdmin : ControllerBase
{
    [HttpPost(Name = "CheckAdmin")]
    public IActionResult Post(string username)
    {
        if (Database.IsAdminExists(username))
        {
            return Ok("User is admin.");
        }

        return BadRequest("Not Valid");

    }
}