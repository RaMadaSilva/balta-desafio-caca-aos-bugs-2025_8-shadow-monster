using BugStore.Application.Mappings.Customers;
using BugStore.Domain.Contracts.IRepositories;

namespace BugStore.Application.Features.Customers.GetByIdCustomer;

public class GetByIdCustomerHandler
{
    private readonly ICustomerRepository _repository;

    public GetByIdCustomerHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetByIdCustomerResponse?> HandleAsync(GetByIdCustomerRequest request)
    {
        var customer = await _repository.GetByIdAsync(request.Id);

        if (customer == null)
            return null;

        return customer.ToGetByIdResponse();
    }
}
