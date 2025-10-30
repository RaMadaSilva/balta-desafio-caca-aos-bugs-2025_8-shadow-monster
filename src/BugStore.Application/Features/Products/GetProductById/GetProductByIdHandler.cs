using BugStore.Application.Mappings.Products;
using BugStore.Domain.Contracts.IRepositories;

namespace BugStore.Application.Features.Products.GetProductById;

public class GetProductByIdHandler
{
    private readonly IProductRepository _repository;

    public GetProductByIdHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetProductByIdResponse?> HandleAsync(GetProductByIdQuery query)
    {
        var product = await _repository.GetByIdAsync(query.Id);

        if (product == null)
            return null;

        // Usar mapping para converter
        return product.ToGetByIdResponse();
    }
}
