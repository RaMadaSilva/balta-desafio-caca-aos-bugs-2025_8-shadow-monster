namespace BugStore.Application.Features.Orders.OrderSearch;

public class OrderSearchResponse
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public List<OrderLineSearchResponse> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class OrderLineSearchResponse
{
    public Guid ProductId { get; set; }
    public string ProductTitle { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Total { get; set; }
}

