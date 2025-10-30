namespace BugStore.Domain.Entities;

public class OrderLine : BaseEntity
{
    public Guid OrderId { get; set; }
    
    public int Quantity { get; set; }
    public decimal Total { get; set; }
    
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}