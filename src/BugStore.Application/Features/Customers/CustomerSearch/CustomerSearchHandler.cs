using BugStore.Application.Features.Customers.GetCustomers;
using BugStore.Application.Mappings.Customers;
using BugStore.Domain.Common;
using BugStore.Domain.Contracts.IRepositories;

namespace BugStore.Application.Features.Customers.CustomerSearch;

public class CustomerSearchHandler
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomerSearchHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResponse<GetCustomersResponse>> HandleAsync(CustomerSearchRequest request)
    {
        var parameters = new CustomerSearchParameters
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone
        };

        var customers = await _unitOfWork.Customers.SearchAsync(parameters);

        var responseItems = customers.Items.Select(c => c.ToGetResponse()).ToList();

        return new PagedResponse<GetCustomersResponse>(
            responseItems,
            customers.TotalCount,
            customers.PageSize,
            customers.CurrentPage
        );
    }
}
