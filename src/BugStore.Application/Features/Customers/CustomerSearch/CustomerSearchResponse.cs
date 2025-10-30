using BugStore.Application.Shared;

namespace BugStore.Application.Features.Customers.CustomerSearch; 

public class CustomerSearchResponse : CustomerResponse
{
    public string Phone { get; set; } = string.Empty;
}
