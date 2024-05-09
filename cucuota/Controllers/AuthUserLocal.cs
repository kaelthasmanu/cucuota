using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace cucuota.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthUserLocal:ControllerBase
{
    private readonly WorkingFiles _workingFiles;
    private readonly ServerProxy _configProxy;
    public AuthUserLocal(IOptions<WorkingFiles> workingFiles, IOptions<ServerProxy> configProxy) 
    {
        _workingFiles = workingFiles.Value;
        _configProxy = configProxy.Value;
    }
    
    [HttpPost("ChangePassword", Name = "ChangePasswordLocal")]
    public IActionResult ChangePassword(User request)
    {
        if (System.IO.File.Exists(_workingFiles.FilePasswordLocal))
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"htpasswd -b -c {_workingFiles.FilePasswordLocal} {request.Username} {request.Password}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            Process proc = new Process() { StartInfo = startInfo };

            proc.Start();

            string output = proc.StandardOutput.ReadToEnd();

            proc.WaitForExit();

            Console.WriteLine("Salida del proceso:");
            Console.WriteLine(output);
            return Ok("Success");
        }
        else
        {
            return BadRequest("Error");
        }
    }
    [HttpPost("Auth", Name = "AuthLocal")]
    public IActionResult AuthPassword(User request, [FromServices] Database database)
    {
        string proxyUrl = $"http://{request.Username}:{request.Password}@{_configProxy.server}:{_configProxy.port}";
        Console.WriteLine(proxyUrl);
        
        ProcessStartInfo startInfo = new ProcessStartInfo()
        {
            FileName = "/bin/bash",
            Arguments =
                $"-c \"curl -x {proxyUrl} http://captive.apple.com\"",
            RedirectStandardOutput = true,
            UseShellExecute = false
        };
        Process proc = new Process() { StartInfo = startInfo };

        proc.Start();

        string output = proc.StandardOutput.ReadToEnd();

        proc.WaitForExit();

        if (output == "<HTML><HEAD><TITLE>Success</TITLE></HEAD><BODY>Success</BODY></HTML>\n")
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
            return BadRequest("Error connection proxy no work");
        }
        
    }
}