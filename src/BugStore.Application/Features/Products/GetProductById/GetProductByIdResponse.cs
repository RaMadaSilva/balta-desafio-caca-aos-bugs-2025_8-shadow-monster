using BugStore.Application.Shared;

namespace BugStore.Application.Features.Products.GetProductById;

public class GetProductByIdResponse 
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public decimal Price { get; set; }
}


