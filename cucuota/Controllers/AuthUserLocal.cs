using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace cucuota.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthUserLocal
{
    private readonly WorkingFiles _workingFiles;
    public AuthUserLocal(IOptions<WorkingFiles> workingFiles) 
    {
        _workingFiles = workingFiles.Value;
    }
    private readonly ILogger<Cuota> _logger;
    
    [HttpGet(Name = "ChangePasswordLocal")]
    public void Post(User request)
    {
        if (File.Exists(_workingFiles.FilePasswordLocal))
        {
            ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = "/bin/bash", Arguments = $"htpasswd -b -c {_workingFiles.FilePasswordLocal} {request.Username} {request.Password}", }; 
            Process proc = new Process() { StartInfo = startInfo, };
            proc.Start();    
        }
        else
        {
            Console.WriteLine("No exist File Local Password");
        }
    }
}