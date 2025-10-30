using FluentValidation;

namespace BugStore.Application.Features.Orders.CreateOrder
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID é obrigatório.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("A ordem deve conter pelo menos um item.");

            RuleForEach(x => x.Items)
                .SetValidator(new OrderLineItemValidator());

            RuleFor(x => x.Items)
                .Must(items => items.Select(i => i.ProductId).Count() == items.Select(i => i.ProductId).Distinct().Count())
                .WithMessage("Não é permitido produtos duplicados na ordem.");
        }
    }

    public class OrderLineItemValidator : AbstractValidator<OrderLineItem>
    {
        public OrderLineItemValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID é obrigatório.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero.");
        }
    }
}


