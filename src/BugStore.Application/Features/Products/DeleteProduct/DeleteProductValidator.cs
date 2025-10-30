using FluentValidation;

namespace BugStore.Application.Features.Products.DeleteProduct;

public class DeleteProductValidator : AbstractValidator<DeleteProductRequest>
{
    public DeleteProductValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id é obrigatório");
    }
}


