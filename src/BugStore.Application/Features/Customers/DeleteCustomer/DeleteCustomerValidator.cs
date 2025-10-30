using FluentValidation;

namespace BugStore.Application.Features.Customers.DeleteCustomer;

public class DeleteCustomerValidator : AbstractValidator<DeleteCustomerRequest>
{
    public DeleteCustomerValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id é obrigatório");
    }
}