using BugStore.Application.Mappings.Products;
using BugStore.Domain.Contracts.IRepositories;
using BugStore.Domain.Entities;
using FluentValidation;

namespace BugStore.Application.Features.Products.CreateProduct;

public class CreateProductHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AbstractValidator<CreateProductRequest> _validator;

    public CreateProductHandler(IUnitOfWork unitOfWork, AbstractValidator<CreateProductRequest> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<CreateProductResponse> HandleAsync(CreateProductRequest request, CancellationToken cancellationToken)
    {
        // Validação
        if (!await ValidationAsync(request))
            throw new ArgumentException("Invalid product data.");

        // Mapear request para entidade
        var product = request.ToEntity();

        var createdProduct = await _unitOfWork.Products.CreateAsync(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Mapear entidade para response
        return createdProduct.ToCreateResponse();
    }

    private async Task<bool> ValidationAsync(CreateProductRequest request)
    {
        var result = await _validator.ValidateAsync(request);

        if (!result.IsValid)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
            return false;
        }

        return true;
    }
}
