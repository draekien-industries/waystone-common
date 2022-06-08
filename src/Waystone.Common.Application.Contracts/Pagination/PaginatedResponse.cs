namespace Waystone.Common.Application.Contracts.Pagination;

/// <summary>
/// Represents a page of records.
/// </summary>
/// <typeparam name="T">The type of the record in the results set.</typeparam>
public class PaginatedResponse<T>
{
    /// <summary>
    /// Gets or sets the previous, current, and next page <see cref="Links"/>.
    /// </summary>
    public Links? Links { get; set; }

    /// <summary>
    /// Gets or sets the total number of records available.
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// Gets or initializes the current page of records of type T.
    /// </summary>
    public IEnumerable<T> Results { get; init; } = new List<T>();
}
