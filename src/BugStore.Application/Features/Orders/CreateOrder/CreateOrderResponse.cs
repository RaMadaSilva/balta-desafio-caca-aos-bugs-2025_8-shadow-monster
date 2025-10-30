namespace BugStore.Application.Features.Orders.CreateOrder
{
    public class CreateOrderResponse
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Total { get; set; }
        public List<OrderLineResponse> Items { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }

    public class OrderLineResponse
    {
        public Guid ProductId { get; set; }
        public string ProductTitle { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}


