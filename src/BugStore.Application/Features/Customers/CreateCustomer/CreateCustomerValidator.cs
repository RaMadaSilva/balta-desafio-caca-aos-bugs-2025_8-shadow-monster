using FluentValidation;

namespace BugStore.Application.Features.Customers.CreateCustomer
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerRequest>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Nome é Obrigatorio.")
                .MaximumLength(100).WithMessage("O Nome não pode Exceder 100 caracteres");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email inválido.");

            RuleFor(x => x.BirthDate)
                .NotEqual(DateTime.Now)
                .WithMessage("Data de nascimento invalida");
        }
    }
}
