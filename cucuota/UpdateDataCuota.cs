using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
namespace cucuota;



public class UpdateDataCuota
{
    private readonly WorkingFiles _workingFiles;
    public UpdateDataCuota(IOptions<WorkingFiles> workingFiles)
    {
        _workingFiles = workingFiles.Value;
    }

    public void RunCuota()
    {
       
            Database database = new Database();
            ReadLog log = new ReadLog();
            
            database.CreateTablesIfNotExist();
            var users = log.ReadFileToUsers(_workingFiles.FullLogFilePath);
            log.ReadFileToTraffic(users, _workingFiles.FullLogFilePath);
            var list = Parsing.ParseUserData(_workingFiles.FullQuoteFilePath);
            List<UserDataJson> userList = JsonConvert.DeserializeObject<List<UserDataJson>>(Database.GetAllUserDataAsJson());
            foreach (var user in userList)
            {
                var matchingUserData = list.Find(userData => userData.Username == user.Name);

                if (matchingUserData != null)
                {
                    if (user.TrafficD /1024  == matchingUserData.Gigabytes || user.TrafficD / 1024  > matchingUserData.Gigabytes)
                    {
                        try
                        {
                            string contenido = File.ReadAllText(_workingFiles.FullBannedFilePath);
                            
                            if (!contenido.Contains(user.Name))
                            {
                                using (StreamWriter writer = new StreamWriter(_workingFiles.FullBannedFilePath, true))
                                {
                                    writer.WriteLine(user.Name);
                                }
                            }
                            else
                            {
                                Console.WriteLine("La línea ya existe en el archivo.");
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