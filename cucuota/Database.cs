using Microsoft.EntityFrameworkCore;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double TrafficD { get; set; }
    public int TrafficW { get; set; }
    public int TrafficM { get; set; }
}

public class Date
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
}

public class Admin
{
    public int Id { get; set; }
    public string Username { get; set; }
}

public class Database : DbContext
{
    private readonly string databasePath = "/Users/manuel/Desktop/cucuota/cucuota/database.db";

    public DbSet<User> Users { get; set; }
    public DbSet<Date> Dates { get; set; }
    public DbSet<Admin> Admins { get; set; }

    public Database(DbContextOptions<Database> options):base(options)
    {
        
    }
    

    public void CreateTablesIfNotExist()
    {
        if (!Users.Any() && !Dates.Any() && !Admins.Any())
        {
            Database.Migrate();
            Console.WriteLine("Tables created successfully.");
        }
        else
        {
            Console.WriteLine("Tables already exist.");
        }
    }

    public void AddOrUpdateUserData(string name, double trafficD, int trafficM, int trafficW)
    {
        var existingUser = Users.FirstOrDefault(u => u.Name == name);

        if (existingUser != null)
        {
            existingUser.TrafficD += trafficD;
            existingUser.TrafficW += trafficW;
            existingUser.TrafficM += trafficM;
        }
        else
        {
            var newUser = new User
            {
                Name = name,
                TrafficD = trafficD,
                TrafficW = trafficW,
                TrafficM = trafficM
            };
            Users.Add(newUser);
        }

        SaveChanges();
    }

    public  bool DoesLastDateTimeExist()
    {
        return Dates.Any();
    }

    public  DateTime GetLastDateTime()
    {
        var lastDate = Dates.OrderByDescending(d => d.Id).FirstOrDefault();

        return lastDate != null ? lastDate.DateTime : DateTime.MinValue;
    }

    public  DateTime AddOrUpdateDateTime(DateTime newDateTime)
    {
        var existingDate = Dates.OrderByDescending(d => d.Id).FirstOrDefault();

        if (existingDate != null)
        {
            existingDate.DateTime = newDateTime;
        }
        else
        {
            var newDate = new Date
            {
                DateTime = newDateTime
            };
            Dates.Add(newDate);
        }

        SaveChanges();

        return newDateTime;
    }

    public  string GetAllUserDataAsJson()
    {
        var userDataList = Users.Select(u => new
        {
            u.Name,
            u.TrafficD,
            u.TrafficW,
            u.TrafficM
        }).ToList();

        var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(userDataList);

        return jsonResult;
    }

    private static bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return System.Text.RegularExpressions.Regex.IsMatch(email, pattern);
    }

    public  bool CreateAdmin(string username)
    {
        if (!IsValidEmail(username))
        {
            // Invalid email, so don't proceed.
            return false;
        }

        if (Admins.Any(a => a.Username == username))
        {
            // Admin with this username already exists.
            return false;
        }

        var newAdmin = new Admin
        {
            Username = username
        };

        Admins.Add(newAdmin);
        SaveChanges();

        return true;
    }

    public  bool IsAdminExists(string username)
    {
        return Admins.Any(a => a.Username == username);
    }

    public  bool VerifyAdmin(string username)
    {
        return IsAdminExists(username);
    }

    public  bool DeleteAdmin(string username)
    {
        if (string.IsNullOrEmpty(username) || !IsValidEmail(username))
        {
            // El nombre de usuario no es válido, no procedemos.
            return false;
        }

        var adminToDelete = Admins.FirstOrDefault(a => a.Username == username);

        if (adminToDelete != null)
        {
            Admins.Remove(adminToDelete);
            SaveChanges();
            return true;
        }

        // El admin no existe, no podemos eliminarlo.
        return false;
    }
}
