namespace BugStore.Application.Features.Reports.RevenueByPeriod; 

public class RevenueByPeriodResponse
{
    public int Year { get; set; }
    public string Month { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }
}
