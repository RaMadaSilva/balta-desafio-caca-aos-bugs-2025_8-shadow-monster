using BugStore.Application.Mappings.Customers;
using BugStore.Domain.Contracts.IRepositories;
using BugStore.Domain.Exceptions;
using FluentValidation;

namespace BugStore.Application.Features.Customers.UpdateCustomer;

public class UpdateCustomerHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AbstractValidator<UpdateCustomerRequest> _validator;

    public UpdateCustomerHandler(IUnitOfWork unitOfWork, AbstractValidator<UpdateCustomerRequest> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<UpdateCustomerResponse> HandleAsync(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        // Validação
        if (!await ValidationAsync(request))
            throw new ArgumentException("Dados do cliente inválidos.");

        // Buscar customer
        var customer = await _unitOfWork.Customers.GetByIdAsync(request.Id);
        if (customer == null)
            throw new NotFoundException($"Customer com ID {request.Id} não foi encontrado");

        // Aplicar updates usando mapping
        customer.ApplyUpdates(request);

        await _unitOfWork.Customers.UpdateAsync(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateCustomerResponse
        {
            Id = customer.Id,
            Message = "Customer atualizado com sucesso"
        };
    }

    private async Task<bool> ValidationAsync(UpdateCustomerRequest request)
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
