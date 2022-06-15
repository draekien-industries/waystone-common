namespace Waystone.Common.Application.Contracts.Pagination;

internal static class PaginationOptions
{
    public const int MinimumLimit = 1;
    public const int MaximumLimit = 100;
    public const int DefaultLimit = 10;
    public const int MinimumCursor = 0;
}
