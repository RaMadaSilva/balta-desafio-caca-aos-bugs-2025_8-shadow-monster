namespace BugStore.Domain.Common; 

public class PaginatedList<TEntity>
{
    public IEnumerable<TEntity> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; } = 10; 
    public int CurrentPage { get; set; } = 1;
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public PaginatedList(IEnumerable<TEntity> items, int totalCount, int pageSize, int currentPage)
    {
        Items = items;
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
    }

    public static PaginatedList<TEntity> ToPagedList(List<TEntity> source, int count, int pageNumber, int pageSize)
    {
        var items = source;
        return new PaginatedList<TEntity>(items, count, pageSize, pageNumber);
    }
}
