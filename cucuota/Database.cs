﻿using System.Text.RegularExpressions;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
namespace cucuota;

public class Database
    {
        private readonly static string databasePath = "database.db";

        public void CreateTablesIfNotExist()
        {
            if (!TableExists("user") && !TableExists("date")  && !TableExists("admins"))
            {
                CreateTable("user",
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                    "name TEXT NOT NULL," +
                    "trafficD DECIMAL NOT NULL," +
                    "trafficW INTEGER NOT NULL," +
                    "trafficM INTEGER NOT NULL");

                CreateTable("date",
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                    "dateTime DATETIME NOT NULL");
                
                CreateTable("admins",
                    "id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                    "username TEXT NOT NULL");

                Console.WriteLine("Tables created successfully.");
            }
            else
            {
                Console.WriteLine("Tables already exist.");
            }
        }

        private void CreateTable(string table, string columns)
        {
            using (var connection = new SqliteConnection($"Data Source={databasePath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"CREATE TABLE {table} ({columns});";
                command.ExecuteNonQuery();
            }
        }

        private bool TableExists(string tableName)
        {
            using (var connection = new SqliteConnection($"Data Source={databasePath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}'";

                using (var reader = command.ExecuteReader())
                {
                    return reader.HasRows;
                }
            }
        }

        public void AddOrUpdateUserData(string name, double trafficD, int trafficM, int trafficW)
{
    using (var connection = new SqliteConnection($"Data Source={databasePath}"))
    {
        connection.Open();
        var command = connection.CreateCommand();
        
        command.CommandText = "SELECT COUNT(*) FROM user WHERE name = $name;";
        command.Parameters.AddWithValue("$name", name);

        int existingUserCount = Convert.ToInt32(command.ExecuteScalar());

        if (existingUserCount > 0)
        {
            command.CommandText =
                "SELECT trafficD, trafficW, trafficM FROM user WHERE name = $name;";
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    double currentTrafficD = reader.GetDouble(0);
                    int currentTrafficW = reader.GetInt32(1);
                    int currentTrafficM = reader.GetInt32(2);
                    
                    trafficD += currentTrafficD;
                    trafficW += currentTrafficW;
                    trafficM += currentTrafficM;
                }
            }
            command.CommandText =
                @"
                UPDATE user
                SET trafficD = $trafficD, trafficW = $trafficW, trafficM = $trafficM
                WHERE name = $name;
                ";
        }
        else
        {
            command.CommandText =
                @"
                INSERT INTO user (name, trafficD, trafficW, trafficM)
                VALUES ($name, $trafficD, $trafficW, $trafficM);
                ";
        }
        
        command.Parameters.AddWithValue("$trafficD", trafficD);
        command.Parameters.AddWithValue("$trafficW", trafficW);
        command.Parameters.AddWithValue("$trafficM", trafficM);
        
        command.ExecuteNonQuery();
    }
}
        public static bool DoesLastDateTimeExist()
        {
            using (var connection = new SqliteConnection($"Data Source={databasePath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = "SELECT COUNT(*) FROM date;";
                var count = Convert.ToInt32(command.ExecuteScalar());

                if (count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public static DateTime GetLastDateTime()
        {
            using (var connection = new SqliteConnection($"Data Source={databasePath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = "SELECT dateTime FROM date ORDER BY id DESC LIMIT 1;";

                var lastDateTime = command.ExecuteScalar();

                if (lastDateTime != null && lastDateTime != DBNull.Value)
                {
                    if (lastDateTime is DateTime)
                    {
                        return (DateTime)lastDateTime;
                    }
                    else if (lastDateTime is string)
                    {
                        if (DateTime.TryParse((string)lastDateTime, out DateTime parsedDateTime))
                        {
                            return parsedDateTime;
                        }
                    }
                }
                return DateTime.MinValue;
            }
        }
        
        public static DateTime AddOrUpdateDateTime(DateTime newDateTime)
        {
            using (var connection = new SqliteConnection($"Data Source={databasePath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();

                // Verificar si ya existe un registro
                command.CommandText = "SELECT id FROM date ORDER BY id DESC LIMIT 1;";
                var existingId = command.ExecuteScalar();

                if (existingId != null && existingId != DBNull.Value)
                {
                    // Si existe, actualiza el registro
                    command.CommandText = "UPDATE date SET dateTime = $dateTime WHERE id = $id;";
                    command.Parameters.AddWithValue("$dateTime", newDateTime);
                    command.Parameters.AddWithValue("$id", existingId);
                    command.ExecuteNonQuery();
                }
                else
                {
                    // Si no existe, agrega un nuevo registro
                    command.CommandText = "INSERT INTO date (dateTime) VALUES ($dateTime);";
                    command.Parameters.AddWithValue("$dateTime", newDateTime);
                    command.ExecuteNonQuery();
                }

                return newDateTime;
            }
        }

        
        public static string GetAllUserDataAsJson()
        {
            using (var connection = new SqliteConnection($"Data Source={databasePath}"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                
                command.CommandText = "SELECT name, trafficD, trafficW, trafficM FROM user;";

                var result = new List<Dictionary<string, object>>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var userData = new Dictionary<string, object>
                        {
                            {"name", reader.GetString(0)},
                            {"trafficD", reader.GetDouble(1)},
                            {"trafficW", reader.GetInt32(2)},
                            {"trafficM", reader.GetInt32(3)}
                        };
                        result.Add(userData);
                    }
                }
                
                var jsonResult = JsonConvert.SerializeObject(result);

                return jsonResult;
            }
        }

        public static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        public static bool CreateAdmin(string username)
        {
            if (!IsValidEmail(username))
            {
                // Invalid email, so don't proceed.
                return false;
            }

            try
            {
                using (var connection = new SqliteConnection($"Data Source={databasePath}"))
                {
                    connection.Open();
            
                    if (IsAdminExists(connection, username))
                    {
                        // Admin with this username already exists.
                        return false;
                    }

                    InsertAdmin(connection, username);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while executing SQL: " + e.Message);
                return false;
            }
        }

        private static bool IsAdminExists(SqliteConnection connection, string username)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(*) FROM admins WHERE username = $username;";
                command.Parameters.AddWithValue("$username", username);

                int existingUserCount = Convert.ToInt32(command.ExecuteScalar());
                return existingUserCount > 0;
            }
        }

        private static void InsertAdmin(SqliteConnection connection, string username)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO admins (username) VALUES ($username);";
                command.Parameters.AddWithValue("$username", username);
                command.ExecuteNonQuery();
            }
        }
        
        public static bool DeleteAdmin(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                // El nombre de usuario no es válido, no procedemos.
                return false;
            }

            try
            {
                using (var connection = new SqliteConnection($"Data Source={databasePath}"))
                {
                    connection.Open();

                    if (!IsAdminExists(connection, username))
                    {
                        // El admin no existe, no podemos eliminarlo.
                        return false;
                    }

                    RemoveAdmin(connection, username);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while executing SQL: " + e.Message);
                return false;
            }
        }

        private static void RemoveAdmin(SqliteConnection connection, string username)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM admins WHERE username = $username;";
                command.Parameters.AddWithValue("$username", username);
                command.ExecuteNonQuery();
            }
        }
    }