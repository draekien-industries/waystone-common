namespace Waystone.Common.Application.Contracts.Pagination;

/// <summary>
/// The links used to navigate between pages in a paginated collection.
/// </summary>
public class Links
{
    /// <summary>
    /// Gets or sets a URI to the current page.
    /// </summary>
    public Uri? Self { get; set; }

    /// <summary>
    /// Gets or sets a URI to the next page.
    /// </summary>
    public Uri? Next { get; set; }

    /// <summary>
    /// Gets or sets a URI to the previous page.
    /// </summary>
    public Uri? Previous { get; set; }
}
