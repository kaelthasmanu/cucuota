using System.Text.RegularExpressions;
namespace cucuota;

public static class Parsing
{
    public static DateTime timestapToDate(double timestamp)
    {
        DateTime dateTime = UnixTimeStampToDateTime(timestamp);
        string formattedDateTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        
        return dateTime;
    }
    private static DateTime UnixTimeStampToDateTime(double unixTimestamp)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds((long)(unixTimestamp * 1000)).DateTime;
    }
    public static List<UserData> ParseUserData(string filePath)
    {
        List<UserData> userDataList = new List<UserData>();

        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    // Utiliza una expresión regular para encontrar el patrón 'nombreusuario <n>Gb/month'
                    Match match = Regex.Match(line, @"(?i)(\S+)\s+(\d+)gb/month");

                    if (match.Success)
                    {
                        string username = match.Groups[1].Value;
                        int gigabytes = int.Parse(match.Groups[2].Value);

                        userDataList.Add(new UserData
                        {
                            Username = username,
                            Gigabytes = gigabytes
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al leer el archivo: {ex.Message}");
        }

        return userDataList;
    }
}