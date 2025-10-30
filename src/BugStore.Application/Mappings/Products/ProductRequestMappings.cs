using BugStore.Application.Features.Products.CreateProduct;
using BugStore.Application.Features.Products.UpdateProduct;
using BugStore.Domain.Entities;

namespace BugStore.Application.Mappings.Products;

public static class ProductRequestMappings
{
    public static Product ToEntity(this CreateProductRequest request)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Slug = request.Slug,
            Price = request.Price
        };
    }

    public static Product ApplyUpdates(this Product product, UpdateProductRequest request)
    {
        product.Title = request.Title;
        product.Description = request.Description;
        product.Slug = request.Slug;
        product.Price = request.Price;
        
        return product;
    }
}

