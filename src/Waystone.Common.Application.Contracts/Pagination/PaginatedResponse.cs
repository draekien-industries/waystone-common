namespace Waystone.Common.Application.Contracts.Pagination;

/// <summary>Represents a page of records.</summary>
/// <typeparam name="T">The type of the record in the results set.</typeparam>
public class PaginatedResponse<T>
{
    /// <summary>The previous, current, and next page <see cref="Links" />.</summary>
    public Links? Links { get; set; }

    /// <summary>The total number of records available.</summary>
    /// <example>100</example>
    public int Total { get; set; }

    /// <summary>The current page of records.</summary>
    public IEnumerable<T> Results { get; init; } = new List<T>();
}
