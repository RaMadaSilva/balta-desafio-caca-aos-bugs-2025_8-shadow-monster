using BugStore.Domain.Common;

namespace BugStore.Application.Features.Customers.CustomerSearch; 

public  class CustomerSearchRequest 
    : RequestParameters
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}
