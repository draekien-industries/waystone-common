namespace Waystone.Common.Application.Contracts.Pagination;

/// <summary>The links used to navigate between pages in a paginated collection.</summary>
public class Links
{
    /// <summary>The URI link to the current page of results.</summary>
    public Uri? Self { get; set; }

    /// <summary>The URI link to the next page of results if it exists.</summary>
    public Uri? Next { get; set; }

    /// <summary>The URI link to the previous page of results if it exists.</summary>
    public Uri? Previous { get; set; }
}
