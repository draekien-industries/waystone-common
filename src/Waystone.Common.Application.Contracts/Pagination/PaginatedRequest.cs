namespace Waystone.Common.Application.Contracts.Pagination;

using MediatR;

/// <summary>The base class for all paginated requests.</summary>
/// <typeparam name="T">The type of the record in the results set.</typeparam>
public class PaginatedRequest<T> : IRequest<PaginatedResponse<T>>
{
    private int? _limit;

    /// <summary>The maximum number of records to return;</summary>
    public int Limit
    {
        get => _limit ?? 10;
        set
        {
            _limit = value switch
            {
                < 1 => 10,
                > 100 => 100,
                var _ => value,
            };
        }
    }

    /// <summary>The offset (from 0) of the first record to return;</summary>
    public int? Cursor { get; set; }
}
