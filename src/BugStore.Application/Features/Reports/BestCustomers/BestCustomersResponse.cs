namespace BugStore.Application.Features.Reports.BestCustomers; 

public class BestCustomersResponse
{
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public int TotalOrders { get; set; }
    public decimal SpentAmount { get; set; }
}
