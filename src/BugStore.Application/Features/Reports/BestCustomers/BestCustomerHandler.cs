using BugStore.Application.Mappings.Reports;
using BugStore.Domain.Common;
using BugStore.Domain.Contracts.IRepositories;

namespace BugStore.Application.Features.Reports.BestCustomers; 

public class BestCustomerHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public BestCustomerHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResponse<BestCustomersResponse>> HandleAsync(BestCustomersRequest request, CancellationToken cancellationToken)
    {
        var parameters = new BestCustomerParameters
        {
            Top = request.Top,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize

        };

        var bestCustomerDto = await _unitOfWork
                                        .Orders
                                        .GetBestCustomerAsync(parameters, cancellationToken);

        return ReportsMapping.ToPagedResponse(bestCustomerDto); 
    }

}
