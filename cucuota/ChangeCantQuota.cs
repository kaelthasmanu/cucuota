using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace cucuota;

public class WorkingFiles
{
    public string LogFile{ get; set; }
    public string QuoteFile{ get; set; }
    public string BannedFile{ get; set; }
    
    public string FullLogFilePath =>  LogFile;
    public string FullQuoteFilePath =>  QuoteFile;
    public string FullBannedFilePath =>  BannedFile;
    
}

public class ChangeCantQuota
{
    private readonly WorkingFiles _workingFiles;
    public ChangeCantQuota(IOptions<WorkingFiles> workingFiles)
    {
        _workingFiles = workingFiles.Value;
    }
    
    public  bool Change(string usernameToChange, int newLimitInGB)
    {       
        string[] lines = File.ReadAllLines(_workingFiles.FullQuoteFilePath);
        
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains(usernameToChange))
            {
                string[] parts = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length >= 2)
                {
                    parts[1] = newLimitInGB + "GB/month";
                    lines[i] = string.Join(" ", parts);
                    break;
                }
            }
            else
            {
                return false;
            }
        }
        File.WriteAllLines(_workingFiles.FullQuoteFilePath, lines);
        Console.WriteLine("Límite de uso mensual actualizado para " + usernameToChange);
        return true;
    }
}