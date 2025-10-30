using BugStore.Application.Mappings.Products;
using BugStore.Domain.Contracts.IRepositories;
using BugStore.Domain.Exceptions;
using FluentValidation;

namespace BugStore.Application.Features.Products.UpdateProduct;

public class UpdateProductHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AbstractValidator<UpdateProductRequest> _validator;

    public UpdateProductHandler(IUnitOfWork unitOfWork, AbstractValidator<UpdateProductRequest> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<UpdateProductResponse> HandleAsync(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        // Validação
        if (!await ValidationAsync(request))
            throw new ArgumentException("Dados do produto inválidos.");

        // Buscar product
        var product = await _unitOfWork.Products.GetByIdAsync(request.Id);

        if (product == null)
            throw new NotFoundException($"Product com ID {request.Id} não foi encontrado");

        // Aplicar updates usando mapping
        product.ApplyUpdates(request);

        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateProductResponse
        {
            Id = product.Id,
            Message = "Product atualizado com sucesso"
        };
    }

    private async Task<bool> ValidationAsync(UpdateProductRequest request)
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
