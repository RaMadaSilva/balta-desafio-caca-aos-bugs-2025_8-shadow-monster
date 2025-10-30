using BugStore.Application.Features.Products.GetProducts;
using BugStore.Application.Mappings.Products;
using BugStore.Domain.Common;
using BugStore.Domain.Contracts.IRepositories;

namespace BugStore.Application.Features.Products.ProductSearch; 

public class ProductSearchHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductSearchHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResponse<ProductSearchResponse>> HandleAsync(ProductSearchRequest request, CancellationToken cancellationToken)
    {
        var parameters = new ProductSearchParameters
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            Title = request.Title,
            Description = request.Description,
            Slug = request.Slug,
            Price = request.Price
        };

        var products = await _unitOfWork.Products.SearchAsync(parameters, cancellationToken);

        var items = products.Items.Select(x=>x.ToProductSearchResponse()).ToList();

        return new PagedResponse<ProductSearchResponse>(items, 
            products.TotalCount, 
            products.PageSize, 
            products.CurrentPage);
    }
}
