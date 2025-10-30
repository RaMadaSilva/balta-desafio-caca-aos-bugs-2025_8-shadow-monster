namespace BugStore.Application.Features.Products.CreateProduct;

public class CreateProductRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public decimal Price { get; set; }
}


