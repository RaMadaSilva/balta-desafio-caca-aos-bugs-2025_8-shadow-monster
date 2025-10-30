using BugStore.Application.Mappings.Customers;
using BugStore.Application.Shared;
using BugStore.Domain.Contracts.IRepositories;
using BugStore.Domain.Entities;
using FluentValidation;

namespace BugStore.Application.Features.Customers.CreateCustomer;

public class CreateCustomerHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AbstractValidator<CreateCustomerRequest> _validator;

    public CreateCustomerHandler(IUnitOfWork unitOfWork, AbstractValidator<CreateCustomerRequest> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<CustomerResponse> HandleAsync(CreateCustomerRequest request, CancellationToken cancellationToken)
    {

        if (!await ValidationAsync(request))
            throw new ArgumentException("Invalid customer data.");

        var customer = request.ToEntity();

        var createdCustomer = await _unitOfWork.Customers.CreateAsync(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return createdCustomer.ToCreateResponse();
    }

    private async Task<bool> ValidationAsync(CreateCustomerRequest request)
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
