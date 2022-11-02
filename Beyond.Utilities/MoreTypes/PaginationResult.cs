// ReSharper disable UnusedMember.Global
namespace Beyond.Utilities.MoreTypes;

public class PaginationResult<T>
{
    public PaginationResult()
    {
        Results = Array.Empty<T>();
    }

    public int FirstItemOnPage => (PageNumber - 1) * PageSize + 1;

    public bool HasNextPage { get; internal set; }
    public bool HasPreviousPage { get; internal set; }
    public bool IsFirstPage { get; internal set; }
    public bool IsLastPage { get; internal set; }

    public int LastItemOnPage => Math.Min(PageNumber * PageSize, TotalCount);

    public int PageCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public IReadOnlyCollection<T> Results { get; set; }
    public int TotalCount { get; set; }
}