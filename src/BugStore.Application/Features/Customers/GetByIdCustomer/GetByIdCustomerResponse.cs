using BugStore.Application.Shared;

namespace BugStore.Application.Features.Customers.GetByIdCustomer;

public class GetByIdCustomerResponse : CustomerResponse
{
    public string Phone { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
}