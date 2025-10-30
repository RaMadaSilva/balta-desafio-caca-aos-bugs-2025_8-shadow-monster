using FluentValidation;

namespace BugStore.Application.Features.Customers.UpdateCustomer;

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id é obrigatório");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MaximumLength(100).WithMessage("O Nome não pode exceder 100 caracteres");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email inválido.");

        RuleFor(x => x.BirthDate)
            .LessThan(DateTime.Now).WithMessage("Data de nascimento inválida");
    }
}