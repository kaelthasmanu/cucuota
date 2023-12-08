using Microsoft.AspNetCore.Mvc;

namespace cucuota.Controllers;

[ApiController]
[Route("[controller]")]

public class CreateUsersAdmin:ControllerBase
{
    //[Authorize]
    [HttpPost(Name = "CreateUsersAdmin")]
    public IActionResult Post(string username, bool admin, [FromServices] Database database)
    {
        if (admin)
        {
            if (database.CreateAdmin(username))
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
    public IActionResult Post(string username, [FromServices] Database database)
    {
        if (database.DeleteAdmin(username))
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
    public IActionResult Post(string username, [FromServices] Database database)
    {
        if (database.IsAdminExists(username))
        {
            return Ok("User is admin.");
        }

        return BadRequest("Not Valid");

    }
}