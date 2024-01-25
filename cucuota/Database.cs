using Microsoft.EntityFrameworkCore;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double TrafficD { get; set; }
    public int TrafficW { get; set; }
    public int TrafficM { get; set; }
}

public class TimeCuotaPref
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Time { get; set; }
    public int Percentage { get; set; }
}

public class SiteQuota
{
    public int Id { get; set; }
    public string Site { get; set; }
    public int ConsumptionMultiplier { get; set; }
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
    public DbSet<User> Users { get; set; }
    public DbSet<Date> Dates { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<SiteQuota> SitesQuotas { get; set; }
    public DbSet<TimeCuotaPref> TimeCuotaPrefs { get; set; }
    
    public Database(DbContextOptions<Database> options):base(options)
    {
        
    }

    public bool AddSiteQuota(string site, int multiplier)
    {
        try
        {
            var existingSite = SitesQuotas.FirstOrDefault(s => s.Site == site);
            if (existingSite != null)
            {
                existingSite.ConsumptionMultiplier = multiplier;
                SaveChanges();
                return true;
            }
            else
            {
                var newSite = new SiteQuota
                {
                    Site = site,
                    ConsumptionMultiplier = multiplier
                };
                SitesQuotas.Add(newSite);
                SaveChanges();
                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ha ocurrido un error agregando el sitio: {e}");
            return false;
        }
    }

    public int GetMultiplier(string site)
    {
        try
        {
            var result = SitesQuotas.FirstOrDefault(s => site.Contains((s.Site)));
            if (result != null)
            {
                return result.ConsumptionMultiplier;    
            }

            return 0;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e}");
            return 0;
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
            return false;
        }

        var adminToDelete = Admins.FirstOrDefault(a => a.Username == username);

        if (adminToDelete != null)
        {
            Admins.Remove(adminToDelete);
            SaveChanges();
            return true;
        }
        return false;
    }
    
    //Crud
    public bool AddTimeCuotaPref(string name, DateTime date, int percentage)
    {
        var existingUser = TimeCuotaPrefs.FirstOrDefault(u => u.Name == name);
        if (existingUser == null)
        {
            var timeCuota = new TimeCuotaPref()
            {
                Name  = name,
                Time = date,
                Percentage = percentage
            };
            TimeCuotaPrefs.Add(timeCuota);
            SaveChanges();
            return true;
        }
        else
        {
            Console.WriteLine("The name is already");
            return false;
        }
    }

    public TimeCuotaPref GetTimeCuotaPrefById(int id)
    {
        return TimeCuotaPrefs.Find(id);
    }

    public IQueryable<TimeCuotaPref> GetAllTimeCuotaPrefs()
    {
        return TimeCuotaPrefs;
    }

    public void UpdateTimeCuotaPref(TimeCuotaPref timeCuotaPref)
    {
        TimeCuotaPrefs.Update(timeCuotaPref);
        SaveChanges();
    }

    public void DeleteTimeCuotaPref(string name)
    {
        var timeCuotaPref = TimeCuotaPrefs.Find(name);
        if (timeCuotaPref != null)
        {
            TimeCuotaPrefs.Remove(timeCuotaPref);
            SaveChanges();
        }
    }
    // End Crud
}
