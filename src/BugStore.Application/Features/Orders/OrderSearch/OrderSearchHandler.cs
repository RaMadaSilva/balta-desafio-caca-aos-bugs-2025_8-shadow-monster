using BugStore.Application.Mappings.Orders;
using BugStore.Domain.Common;
using BugStore.Domain.Contracts.IRepositories;

namespace BugStore.Application.Features.Orders.OrderSearch;

public class OrderSearchHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderSearchHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResponse<OrderSearchResponse>> HandleAsync(OrderSearchRequest request)
    {
        var parameters = new OrderParameters
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            Id = request.Id,
            CustomerName = request.CustomerName,
            CustomerEmail = request.CustomerEmail,
            CustomerPhone = request.CustomerPhone,
            ProductTitle = request.ProductTitle,
            ProductDescription = request.ProductDescription,
            ProductSlug = request.ProductSlug,
            ProductPriceStart = request.ProductPriceStart,
            ProductPriceEnd = request.ProductPriceEnd,
            CreatedAtStart = request.CreatedAtStart,
            CreatedAtEnd = request.CreatedAtEnd,
            UpdatedAtStart = request.UpdatedAtStart,
            UpdatedAtEnd = request.UpdatedAtEnd
        };

        var orders = await _unitOfWork.Orders.SearchAsync(parameters);

        var responseItems = orders.Items.Select(order => new OrderSearchResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            CustomerName = order.Customer?.Name ?? string.Empty,
            Total = order.Lines?.Sum(l => l.Total) ?? 0,
            Items = order.Lines?.Select(l => new OrderLineSearchResponse
            {
                ProductId = l.ProductId,
                ProductTitle = l.Product?.Title ?? string.Empty,
                Quantity = l.Quantity,
                Price = l.Product?.Price ?? 0,
                Total = l.Total
            }).ToList() ?? new List<OrderLineSearchResponse>(),
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        }).ToList();

        return new PagedResponse<OrderSearchResponse>(
            responseItems,
            orders.TotalCount,
            orders.PageSize,
            orders.CurrentPage
        );
    }
}

