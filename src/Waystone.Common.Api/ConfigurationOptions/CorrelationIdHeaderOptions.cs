namespace Waystone.Common.Api.ConfigurationOptions;

/// <summary>Options for configuring the correlation id header.</summary>
public class CorrelationIdHeaderOptions
{
    /// <summary>The configuration section containing the options.</summary>
    public const string SectionName = "CorrelationIdHeader";

    /// <summary>The default header name if the configuration section does not exist.</summary>
    public const string DefaultHeaderName = "X-Correlation-ID";

    /// <summary>The header name of the correlation id header.</summary>
    public string HeaderName { get; init; } = DefaultHeaderName;

    /// <summary>Should the correlation id header be added to the response.</summary>
    public bool IncludeInResponse { get; init; } = true;
}
