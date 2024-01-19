using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
namespace cucuota;

public class ReadLog
    {
        public ReadLog(Database database, ILogger<ReadLog> logger)
        {
            _database = database;
            _logger = logger;
        }
        private readonly Database _database ;
        private double gigabytes = 0;
        private double gigabytesForUser = 0;
        private readonly ILogger<ReadLog> _logger;
   
        public HashSet<string> ReadFileToUsers(string filePath)
        {
            HashSet<string> usersUnicos = new HashSet<string>();
            _logger.LogInformation("Inciando lectura del log ");
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
            int multiplier = 0;
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
                                //Console.WriteLine(splitLine[6]);//Sites 
                                if (_database.GetMultiplier(splitLine[6]) > 0)
                                {
                                    multiplier = _database.GetMultiplier(splitLine[6]);
                                }
                                else if (_database.GetMultiplier(splitLine[6]) == 0)
                                {
                                    multiplier = 0;
                                }
                                else
                                {
                                    multiplier = _database.GetMultiplier(splitLine[6]);
                                }
                                if (_database.DoesLastDateTimeExist() && DateTime.Compare(Parsing.timestapToDate(double.Parse(splitLine[0])),
                                        _database.GetLastDateTime()) > 0)
                                    {
                                        //Sum porcent to total ((% / 100) * total) + total)
                                        gigabytesForUser += multiplier > 0 ? (multiplier / 100) * (Math.Round(double.Parse(splitLine[4]) / 1024 / 1024, MidpointRounding.AwayFromZero)) + Math.Round(double.Parse(splitLine[4]) / 1024 / 1024, MidpointRounding.AwayFromZero) : ( multiplier == 0 ? Math.Round(double.Parse(splitLine[4]) / 1024 / 1024, MidpointRounding.AwayFromZero) : (multiplier / 100) * (Math.Round(double.Parse(splitLine[4]) / 1024 / 1024, MidpointRounding.AwayFromZero)) + Math.Round(double.Parse(splitLine[4]) / 1024 / 1024, MidpointRounding.AwayFromZero));
                                    }
                                else if (!_database.DoesLastDateTimeExist())
                                {
                                    //Sum porcent to total ((% / 100) * total) + total)
                                    gigabytesForUser += multiplier > 0 ? (multiplier / 100) * (Math.Round(double.Parse(splitLine[4]) / 1024 / 1024, MidpointRounding.AwayFromZero)) + Math.Round(double.Parse(splitLine[4]) / 1024 / 1024, MidpointRounding.AwayFromZero) : ( multiplier == 0 ? Math.Round(double.Parse(splitLine[4]) / 1024 / 1024, MidpointRounding.AwayFromZero) : (multiplier / 100) * (Math.Round(double.Parse(splitLine[4]) / 1024 / 1024, MidpointRounding.AwayFromZero)) + Math.Round(double.Parse(splitLine[4]) / 1024 / 1024, MidpointRounding.AwayFromZero));
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
                
                _database.AddOrUpdateUserData(user, gigabytesForUser, 0, 0);
                
                gigabytesForUser = 0;
            }
            Console.WriteLine("Finish ReadLog adding to database...");
            _database.AddOrUpdateDateTime(Parsing.timestapToDate(double.Parse(splitLine[0])));
        }
    }
