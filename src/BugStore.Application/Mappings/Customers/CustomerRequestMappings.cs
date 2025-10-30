using BugStore.Application.Features.Customers.CreateCustomer;
using BugStore.Application.Features.Customers.UpdateCustomer;
using BugStore.Domain.Entities;

namespace BugStore.Application.Mappings.Customers;

public static class CustomerRequestMappings
{

    public static Customer ToEntity(this CreateCustomerRequest request)
    {
        return new Customer
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            BirthDate = request.BirthDate
        };
    }

    public static Customer ApplyUpdates(this Customer customer, UpdateCustomerRequest request)
    {
        customer.Name = request.Name;
        customer.Email = request.Email;
        customer.Phone = request.Phone;
        customer.BirthDate = request.BirthDate;
        
        return customer;
    }
}

