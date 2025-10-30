using BugStore.Application.Features.Customers.CreateCustomer;
using BugStore.Application.Features.Customers.GetCustomers;
using BugStore.Application.Features.Customers.GetByIdCustomer;
using BugStore.Domain.Entities;

namespace BugStore.Application.Mappings.Customers;

public static class CustomerMappings
{
    public static CreateCustomerResponse ToCreateResponse(this Customer customer)
    {
        return new CreateCustomerResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static GetCustomersResponse ToGetResponse(this Customer customer)
    {
        return new GetCustomersResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            BirthDate = customer.BirthDate
        };
    }

    public static GetByIdCustomerResponse ToGetByIdResponse(this Customer customer)
    {
        return new GetByIdCustomerResponse
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Phone = customer.Phone,
            BirthDate = customer.BirthDate
        };
    }

    public static List<GetCustomersResponse> ToGetResponseList(this IEnumerable<Customer> customers)
    {
        return customers.Select(c => c.ToGetResponse()).ToList();
    }
}

