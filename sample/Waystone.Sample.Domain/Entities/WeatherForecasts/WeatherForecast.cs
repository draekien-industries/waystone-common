namespace Waystone.Sample.Domain.Entities.WeatherForecasts;

public class WeatherForecast
{
    public WeatherForecast(DateTime dateTime, int temperatureC, string summary)
    {
        Id = Guid.NewGuid();
        DateTime = dateTime;
        TemperatureC = temperatureC;
        TemperatureF = 32 + (int)(temperatureC / 0.5556);
        Summary = summary;
    }

    public Guid Id { get; }

    public DateTime DateTime { get; }

    public int TemperatureC { get; }

    public int TemperatureF { get; }

    public string? Summary { get; }
}
