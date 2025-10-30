namespace BugStore.Domain.Common;

public class PagedResponse<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; } = 10;
    public int CurrentPage { get; set; } = 1;
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public PagedResponse(IEnumerable<T> items, int totalCount, int pageSize, int currentPage)
    {
        Items = items;
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
    }
}

