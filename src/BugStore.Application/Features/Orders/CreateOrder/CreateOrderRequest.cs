namespace BugStore.Application.Features.Orders.CreateOrder;

public class CreateOrderRequest
{
    public Guid CustomerId { get; set; }
    public List<OrderLineItem> Items { get; set; } = new();
}

public class OrderLineItem
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}


