namespace Waystone.Common.Api.ConfigurationOptions;

public class CorrelationIdHeaderOptions
{
    public const string SectionName = "CorrelationIdHeader";

    public const string DefaultHeaderName = "X-Correlation-ID";

    public string HeaderName { get; init; } = DefaultHeaderName;

    public bool IncludeInResponse { get; init; } = true;
}
