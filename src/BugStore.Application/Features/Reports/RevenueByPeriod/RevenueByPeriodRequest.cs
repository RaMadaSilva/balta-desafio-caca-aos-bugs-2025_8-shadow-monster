using BugStore.Domain.Common;

namespace BugStore.Application.Features.Reports.RevenueByPeriod; 

public class RevenueByPeriodRequest : RequestParameters
{
    public DateTime StartPeriod { get; set; }
    public DateTime EndPeriod { get; set; }
}
