using BugStore.Domain.Common;
using BugStore.Domain.Entities;

namespace BugStore.Domain.Contracts.IRepositories; 

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PaginatedList<Customer>> GetAllAsync(RequestParameters parameters, CancellationToken cancellationToken = default);
    Task<PaginatedList<Customer>> SearchAsync(CustomerSearchParameters parameters, CancellationToken cancellationToken = default);
    Task<Customer> CreateAsync(Customer customer);
    Task<Customer> UpdateAsync(Customer customer);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}
