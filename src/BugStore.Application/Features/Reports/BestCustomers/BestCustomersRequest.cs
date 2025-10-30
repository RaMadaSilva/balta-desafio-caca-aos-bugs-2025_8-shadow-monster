using BugStore.Domain.Common;

namespace BugStore.Application.Features.Reports.BestCustomers; 

public class BestCustomersRequest: RequestParameters
{
    public int Top { get; set; }
}
