namespace cucuota;

public class Cuota
{
    /*public string Name { get; set; }
    public double TrafficD { get; set; }
    public int TrafficW { get; set; }
    public int TrafficM { get; set; }*/
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}