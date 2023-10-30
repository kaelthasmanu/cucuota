using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
namespace cucuota;

public class ReadLog
    {
        private Database _database = new Database();
        private double gigabytes = 0;
        private double gigabytesForUser = 0;
        //private readonly LogOptions _options;

        /*public ReadLog(IOptions<LogOptions> options)
        {
            _options = options.Value;
        }*/
        public HashSet<string> ReadFileToUsers(string filePath )
        {
            HashSet<string> usersUnicos = new HashSet<string>();

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!line.Contains("HIER_NONE"))
                        {
                            string cleanedLine = Regex.Replace(line, @"\s+", " ");
                            string[] splitLine = cleanedLine.Split(" ");
                            usersUnicos.Add(splitLine[7]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception error !!: " + e.Message);
            }

            return usersUnicos;
        }

        public void ReadFileToTraffic(HashSet<string> usersUnicos, string filePath)
        {
            string[] splitLine = new string[10];
            foreach (var user in usersUnicos)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (!line.Contains("HIER_NONE") && line.Contains(user))
                            {
                                string cleanedLine = Regex.Replace(line, @"\s+", " ");
                                splitLine = cleanedLine.Split(" ");
                                if (Database.DoesLastDateTimeExist() == true)
                                {
                                    if (DateTime.Compare(Parsing.timestapToDate(double.Parse(splitLine[0])),
                                            Database.GetLastDateTime()) > 0)
                                    {
                                        gigabytesForUser += double.Parse(splitLine[4]);
                                    }
                                }
                                else
                                {
                                    gigabytesForUser += double.Parse(splitLine[4]);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error {e}");
                }
                
                    
                //Save traffic in MB
                _database.AddOrUpdateUserData(user, Math.Round(gigabytesForUser / 1024 / 1024, 2), 0, 0); 
                gigabytesForUser = 0;
            }
            Console.WriteLine("Finish ReadLog adding to database...");
            Database.AddOrUpdateDateTime(Parsing.timestapToDate(double.Parse(splitLine[0])));
            //Console.WriteLine($"La cantidad total de Gigas es de {Math.Round(gigabytes / 1024 / 1024 / 1024)}GB");
        }
    }
