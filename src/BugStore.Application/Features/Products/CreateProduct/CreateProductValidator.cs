using FluentValidation;

namespace BugStore.Application.Features.Products.CreateProduct
{
    public class CreateProductValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Título é obrigatório.")
                .MaximumLength(200).WithMessage("O Título não pode exceder 200 caracteres");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug é obrigatório.")
                .MaximumLength(200).WithMessage("O Slug não pode exceder 200 caracteres");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("O Preço deve ser maior que zero.");
        }
    }
}


