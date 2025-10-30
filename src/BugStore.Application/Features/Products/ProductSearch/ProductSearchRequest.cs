using BugStore.Domain.Common;

namespace BugStore.Application.Features.Products.ProductSearch; 

public class ProductSearchRequest : RequestParameters
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Slug { get; set; }
    public decimal? Price { get; set; }
}
