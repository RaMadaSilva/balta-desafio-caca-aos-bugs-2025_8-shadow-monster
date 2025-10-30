using BugStore.Application.Mappings.Customers;
using BugStore.Domain.Common;
using BugStore.Domain.Contracts.IRepositories;

namespace BugStore.Application.Features.Customers.GetCustomers;

public class GetCustomersHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCustomersHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResponse<GetCustomersResponse>> HandleAsync(GetCustomersRequest query)
    {
        var customers = await _unitOfWork.Customers.GetAllAsync(query);

        var responseItems = customers.Items.Select(c => c.ToGetResponse()).ToList();

        return new PagedResponse<GetCustomersResponse>(
            responseItems,
            customers.TotalCount,
            customers.PageSize,
            customers.CurrentPage
        );
    }
}
