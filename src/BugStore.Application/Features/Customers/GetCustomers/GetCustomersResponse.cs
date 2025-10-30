using BugStore.Application.Shared;

namespace BugStore.Application.Features.Customers.GetCustomers;

public class GetCustomersResponse : CustomerResponse
{
    public string Phone { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
}