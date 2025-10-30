using BugStore.Domain.Contracts.IRepositories;

namespace BugStore.Application.Features.Orders.GetOrderById;

public class GetOrderByIdHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public GetOrderByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GetOrderByIdResponse?> HandleAsync(GetOrderByIdRequest query)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(query.Id);

        if (order == null)
            return null;

        return new GetOrderByIdResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            CustomerName = order.Customer?.Name ?? string.Empty,
            Total = order.Lines?.Sum(l => l.Total) ?? 0,
            Items = order.Lines?.Select(l => new OrderLineResponse
            {
                ProductId = l.ProductId,
                ProductTitle = l.Product?.Title ?? string.Empty,
                Quantity = l.Quantity,
                Price = l.Product?.Price ?? 0,
                Total = l.Total
            }).ToList() ?? new List<OrderLineResponse>(),
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        };
    }
}
