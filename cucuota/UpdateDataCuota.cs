using Microsoft.Extensions.Options;
using Newtonsoft.Json;
namespace cucuota;

public class UserData
{
    public string? Username { get; set; }
    public int Gigabytes { get; set; }
    public string? Name { get; set; }
    public double TrafficD { get; set; }
    public int TrafficW { get; set; }
    public int TrafficM { get; set; }
}
public class UpdateDataCuota
{
    private readonly WorkingFiles _workingFiles;
    private readonly ReadLog _log;
    private readonly Database _database;
    public UpdateDataCuota(IOptions<WorkingFiles> workingFiles, ReadLog log , Database database) 
    {
        _workingFiles = workingFiles.Value;
        _log = log;
        _database = database;
    }
    

    public void RunCuota()
    {
        var users = _log.ReadFileToUsers(_workingFiles.FullLogFilePath);
        _log.ReadFileToTraffic(users, _workingFiles.FullLogFilePath);
        var list = Parsing.ParseUserData(_workingFiles.FullQuoteFilePath);
        List<UserData>? userList = JsonConvert.DeserializeObject<List<UserData>>(_database.GetAllUserDataAsJson());
        if (userList != null)
            foreach (var user in userList)
            {
                var matchingUserData = list.Find(userData => userData.Username == user.Name);
                var all = list.Find(userData => userData.Username == "*");

                if (matchingUserData != null)
                {
                    if (user.TrafficD / 1024 == matchingUserData.Gigabytes ||
                        user.TrafficD / 1024 > matchingUserData.Gigabytes)
                    {
                        try
                        {
                            string contenido = File.ReadAllText(_workingFiles.FullBannedFilePath);

                            if (user.Name != null && !contenido.Contains(user.Name))
                            {
                                using (StreamWriter writer = new StreamWriter(_workingFiles.FullBannedFilePath, true))
                                {
                                    writer.WriteLine(user.Name);
                                }
                            }
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine("Ocurrió un error al escribir en el archivo: " + e.Message);
                        }
                    }
                    else if (user.TrafficD / 1024 < matchingUserData.Gigabytes)
                    {
                        try
                        {
                            string filePath = _workingFiles.FullBannedFilePath;
                            List<string> lines = new List<string>(File.ReadAllLines(filePath));

                            if (user.Name != null)
                            {
                                int index = lines.FindIndex(line => line == user.Name);

                                if (index != -1)
                                {
                                    lines.RemoveAt(index);
                                    File.WriteAllLines(filePath, lines);
                                }
                            }
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine("Ocurrió un error al escribir en el archivo: " + e.Message);
                        }
                    }
                }
                else
                {
                    if ((user.TrafficD / 1024 == all.Gigabytes && user.Name != "-") || (user.TrafficD / 1024 > all.Gigabytes && user.Name != "-"))
                    {
                        try
                        {
                            string contenido = File.ReadAllText(_workingFiles.FullBannedFilePath);

                            if (user.Name != null && !contenido.Contains(user.Name))
                            {
                                using (StreamWriter writer = new StreamWriter(_workingFiles.FullBannedFilePath, true))
                                {
                                    writer.WriteLine(user.Name);
                                }
                            }
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine("Ocurrió un error al escribir en el archivo: " + e.Message);
                        }
                    }
                    else if (user.TrafficD / 1024 < all.Gigabytes && user.Name != "-")
                    {
                        try
                        {
                            string filePath = _workingFiles.FullBannedFilePath;
                            List<string> lines = new List<string>(File.ReadAllLines(filePath));

                            if (user.Name != null)
                            {
                                int index = lines.FindIndex(line => line == user.Name);

                                if (index != -1)
                                {
                                    lines.RemoveAt(index);
                                    File.WriteAllLines(filePath, lines);
                                }
                            }
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine("Ocurrió un error al escribir en el archivo: " + e.Message);
                        }
                    }
                }
            }
    }
}