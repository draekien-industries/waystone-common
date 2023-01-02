namespace Waystone.Sample.Domain.Entities.WeatherForecasts;

using Common.Domain.Contracts.Primitives;

public class WeatherForecast : Entity<Guid>
{
    public WeatherForecast(Guid id, DateTime dateTime, int temperatureC, ForecastSummary summary) : base(id)
    {
        DateTime = dateTime;
        TemperatureC = temperatureC;
        Summary = summary;
    }

    public WeatherForecast(DateTime dateTime, int temperatureC, ForecastSummary summary) : base(Guid.NewGuid())
    {
        DateTime = dateTime;
        TemperatureC = temperatureC;
        Summary = summary;
    }

    public DateTime DateTime { get; }

    public int TemperatureC { get; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public ForecastSummary? Summary { get; }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetSignatureComponents()
    {
        yield return DateTime;
        yield return TemperatureC;
        yield return Summary;
    }
}
