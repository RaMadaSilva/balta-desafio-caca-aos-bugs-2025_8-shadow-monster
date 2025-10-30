using BugStore.Application.Mappings.Products;
using BugStore.Domain.Contracts.IRepositories;

namespace BugStore.Application.Features.Products.GetProducts;

public class GetProductsHandler
{
    private readonly IProductRepository _repository;

    public GetProductsHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<GetProductsResponse>> HandleAsync(GetProductsRequest query)
    {
        var products = await _repository.GetAllAsync(query);

        // Usar mapping para converter
        return products.Items.Select(p => p.ToGetResponse()).ToList();
    }
}
