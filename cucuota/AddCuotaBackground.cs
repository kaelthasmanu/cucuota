using Microsoft.Extensions.Options;

namespace cucuota;

public class TimeReadLog
{
    public string Time { get; set; }

    public string time => Time;
}

public class AddCuotaBackground:BackgroundService
{
    private readonly IServiceProvider services;
    private readonly TimeReadLog _time;
    public AddCuotaBackground(IServiceProvider services, IOptions<TimeReadLog> time)
    {
        _time = time.Value;
        this.services = services;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = services.CreateScope();
        UpdateDataCuota _updateDataCuota = scope.ServiceProvider.GetRequiredService<UpdateDataCuota>();
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(int.Parse(_time.time),stoppingToken);
            try
            {
                _updateDataCuota.RunCuota();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error Service Background Quota: {e}");   
            }
            await Task.Delay(9000,stoppingToken);
        }
    }
}