using BugStore.Application.Features.Reports.BestCustomers;
using BugStore.Domain.Common;
using BugStore.Domain.DataTransferObject;

namespace BugStore.Application.Mappings.Reports; 

public static class ReportsMapping
{
    public static PagedResponse<BestCustomersResponse> ToPagedResponse(PaginatedList<BestCustomerDto> dto)
    {
        return new PagedResponse<BestCustomersResponse>(
            dto.Items.Select(x => new BestCustomersResponse
            {
                CustomerName = x.CustomerName,
                CustomerEmail = x.CustomerEmail,
                TotalOrders = x.TotalOrders,
                SpentAmount = x.SpentAmount
            }),
            dto.TotalCount,
            dto.PageSize,
            dto.CurrentPage);
    }
}
