// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable CheckNamespace

namespace Beyond.Utilities.MoreTypes;

public static class PaginationExtensions
{
    public static PaginationResult<T> ToPage<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        if (query == null) throw new ArgumentNullException(nameof(query));
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, "pageNumber cannot be less than 1.");
        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "pageSize cannot be less than 1.");

        var result = new PaginationResult<T>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = query.Count(),
            HasPreviousPage = pageNumber > 1,
            IsFirstPage = pageNumber == 1
        };

        var pageCount = result.TotalCount > 0 ? (double)result.TotalCount / pageSize : 0;
        result.PageCount = (int)Math.Ceiling(pageCount);

        result.HasNextPage = pageNumber < result.PageCount;
        result.IsLastPage = pageNumber >= result.PageCount;

        var skip = (pageNumber - 1) * pageSize;
        result.Results = query.Skip(skip)
                            .Take(pageSize)
                            .ToArray();

        return result;
    }

    public static PaginationResult<T> ToPage<T>(this IEnumerable<T> list, int pageNumber, int pageSize)
    {
        if (list == null) throw new ArgumentNullException(nameof(list));
        var query = list.AsQueryable();
        return query.ToPage(pageNumber, pageSize);
    }

    public static PaginationResult<T> ToPage<T>(this T[] list, int pageNumber, int pageSize)
    {
        if (list == null) throw new ArgumentNullException(nameof(list));
        var query = list.AsQueryable();
        return query.ToPage(pageNumber, pageSize);
    }
}