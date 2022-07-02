namespace Waystone.Common.Application.Contracts.Pagination;

using MediatR;
using static PaginationOptions;

/// <summary>The base class for all paginated requests.</summary>
/// <typeparam name="T">The type of the record in the results set.</typeparam>
public abstract class PaginatedRequest<T> : IRequest<PaginatedResponse<T>>
{
    private readonly int? _limit;

    /// <summary>The maximum number of records to return;</summary>
    /// <example>10</example>
    public int Limit
    {
        get => _limit ?? DefaultLimit;
        init
        {
            _limit = value switch
            {
                < MinimumLimit => DefaultLimit,
                > MaximumLimit => MaximumLimit,
                var _ => value,
            };
        }
    }

    /// <summary>The offset (from 0) of the first record to return;</summary>
    /// <example>0</example>
    public int Cursor { get; set; } = MinimumCursor;
}
