using BugStore.Domain.Contracts.IRepositories;
using BugStore.Domain.Entities;
using BugStore.Domain.Exceptions;
using FluentValidation;

namespace BugStore.Application.Features.Orders.CreateOrder;

public class CreateOrderHandler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AbstractValidator<CreateOrderRequest> _validator;

    public CreateOrderHandler(IUnitOfWork unitOfWork,
        AbstractValidator<CreateOrderRequest> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<CreateOrderResponse> HandleAsync(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        // Validação
        if (!await ValidationAsync(request))
            throw new ArgumentException("Invalid order data.");

        // Validar se o customer existe
        var customer = await _unitOfWork.Customers.GetByIdAsync(request.CustomerId);

        if (customer == null)
            throw new NotFoundException($"Customer com ID {request.CustomerId} não foi encontrado");

 

        // Criar ordem
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Lines = new List<OrderLine>()
        };

        decimal total = 0;
        var orderLines = new List<OrderLineResponse>();

        var productIds = request.Items.Select(i => i.ProductId).ToList();
        var products = (await _unitOfWork.Products.GetByIdsAsync(productIds)).ToList();

        // Criar linhas da ordem
        foreach (var item in request.Items)
        {
            var product = products.First(p => p.Id == item.ProductId);
            var lineTotal = product.Price * item.Quantity;
            total += lineTotal;

            var orderLine = new OrderLine
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Total = lineTotal
            };

            order.Lines.Add(orderLine);

            orderLines.Add(new OrderLineResponse
            {
                ProductId = product.Id,
                ProductTitle = product.Title,
                Quantity = item.Quantity,
                Price = product.Price,
                Total = lineTotal
            });
        }

        await _unitOfWork.Orders.CreateAsync(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Retornar resposta
        return new CreateOrderResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Total = total,
            Items = orderLines,
            CreatedAt = order.CreatedAt
        };
    }

    private async Task<bool> ValidationAsync(CreateOrderRequest request)
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
