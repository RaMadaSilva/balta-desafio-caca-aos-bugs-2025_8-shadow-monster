namespace BugStore.Domain.Common; 

public class ProductSearchParameters 
    : RequestParameters
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Slug { get; set; }
    public decimal? Price { get; set; }
}
