using BugStore.Application.Features.Orders.CreateOrder;
using BugStore.Application.Features.Orders.GetOrderById;
using BugStore.Domain.Entities;

namespace BugStore.Application.Mappings.Orders;

public static class OrderMappings
{

    public static CreateOrderResponse ToCreateResponse(
        this Order order, 
        decimal total, 
        List<Features.Orders.CreateOrder.OrderLineResponse> items)
    {
        return new CreateOrderResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Total = total,
            Items = items,
            CreatedAt = order.CreatedAt
        };
    }

    public static GetOrderByIdResponse ToGetByIdResponse(this Order order, decimal total)
    {
        return new GetOrderByIdResponse
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Total = total,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt
        };
    }

    public static Features.Orders.CreateOrder.OrderLineResponse ToCreateOrderLineResponse(
        this OrderLine line, 
        string productTitle, 
        decimal price)
    {
        return new Features.Orders.CreateOrder.OrderLineResponse
        {
            ProductId = line.ProductId,
            ProductTitle = productTitle,
            Quantity = line.Quantity,
            Price = price,
            Total = line.Total
        };
    }

    public static Features.Orders.GetOrderById.OrderLineResponse ToGetOrderLineResponse(
        this OrderLine line, 
        string productTitle, 
        decimal price)
    {
        return new Features.Orders.GetOrderById.OrderLineResponse
        {
            ProductId = line.ProductId,
            ProductTitle = productTitle,
            Quantity = line.Quantity,
            Price = price,
            Total = line.Total
        };
    }
}

