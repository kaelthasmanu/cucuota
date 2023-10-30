using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
namespace cucuota;

public class AddCuotaBackground:BackgroundService
{
    public AddCuotaBackground(UpdateDataCuota updateDataCuota)
    {
        _updateDataCuota = updateDataCuota;
    }
    private readonly UpdateDataCuota _updateDataCuota;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000,stoppingToken);
            try
            {
                _updateDataCuota.RunCuota();
            }
            catch (Exception e)
            {
                Console.WriteLine("File not found");   
            }
            await Task.Delay(9000,stoppingToken);
        }
    }
}