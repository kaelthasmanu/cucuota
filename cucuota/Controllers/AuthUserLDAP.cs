using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace cucuota.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthUserLDAP:ControllerBase
{
    [HttpPost(Name = "AuthUserLDAP")]
    public IActionResult Post(User request, [FromServices] LDAPUtils ldapUtils, [FromServices] Database database)
    {
        bool resp = ldapUtils.AuthenticateUser(request.Username, request.Password);
        if (resp)
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            if (database.VerifyAdmin(request.Username))
            {
                var data = new Dictionary<string, string>()
                {
                    { "message", "Success" },
                    {"accessToken", token},
                    {"user", request.Username},
                    {"admin", "true"}
                };
                return Ok(JsonConvert.SerializeObject(data));    
            }
            else
            {
                var data = new Dictionary<string, string>()
                {
                    { "message", "Success" },
                    {"accessToken", token},
                    {"user", request.Username}
                };
                return Ok(JsonConvert.SerializeObject(data));
            }
        }
        else
        {
            var data = new Dictionary<string, string>()
            {
                { "message", "Credential Invalid" },
                {   "user" , request.Username },
            };
            return BadRequest(JsonConvert.SerializeObject(data));
        }
    }
}