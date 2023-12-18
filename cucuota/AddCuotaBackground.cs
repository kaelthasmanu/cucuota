using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
namespace cucuota;

public class AddCuotaBackground:BackgroundService
{
    private readonly IServiceProvider services;
    public AddCuotaBackground(IServiceProvider services)
    {
        this.services = services;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = services.CreateScope();
        UpdateDataCuota _updateDataCuota = scope.ServiceProvider.GetRequiredService<UpdateDataCuota>();
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(20000,stoppingToken);
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