using BugStore.Domain.Contracts.IRepositories;
using BugStore.Domain.Exceptions;
using FluentValidation;

namespace BugStore.Application.Features.Products.DeleteProduct;

public class DeleteProductHandler
{
    private readonly IUnitOfWork _unitofWork;
    private readonly AbstractValidator<DeleteProductRequest> _validator;

    public DeleteProductHandler(IUnitOfWork unitOfWork, AbstractValidator<DeleteProductRequest> validator)
    {
        _unitofWork = unitOfWork;
        _validator = validator;
    }

    public async Task<DeleteProductResponse> HandleAsync(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        // Validação
        if (!await ValidationAsync(request))
            throw new ArgumentException("ID Inválido");

        var deleted = await _unitofWork.Products.DeleteAsync(request.Id);

        if (!deleted)
            throw new NotFoundException($"Product com ID {request.Id} não foi encontrado");

        await _unitofWork.SaveChangesAsync(cancellationToken);

        return new DeleteProductResponse
        {
            Id = request.Id,
            Message = "Product deletado com sucesso"
        };
    }

    private async Task<bool> ValidationAsync(DeleteProductRequest request)
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
