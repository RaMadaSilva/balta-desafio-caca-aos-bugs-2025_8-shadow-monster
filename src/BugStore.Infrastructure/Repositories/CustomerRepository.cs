using BugStore.Domain.Common;
using BugStore.Domain.Contracts.IRepositories;
using BugStore.Domain.Entities;
using BugStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Infrastructure.Repositories;

public class CustomerRepository
    : BaseRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context)
        : base(context) { }
    public Task<Customer> CreateAsync(Customer customer)
    {
        Add(customer);
        return Task.FromResult(customer);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var customer = await GetByCondition(x => x.Id == id, true)
             .FirstOrDefaultAsync();

        if (customer == null)
            return false;

        Delete(customer);
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
        => await _context.Customers.AnyAsync(c => c.Id == id);


    public async Task<PaginatedList<Customer>> GetAllAsync(RequestParameters parameters, CancellationToken cancellationToken = default)
    {
        var query = GetAll(false);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query.Skip((parameters.PageNumber - 1) * parameters.PageSize)
                            .Take(parameters.PageSize)
                            .ToListAsync(cancellationToken);

        return PaginatedList<Customer>.ToPagedList(items, totalCount, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<PaginatedList<Customer>> SearchAsync(CustomerSearchParameters parameters, CancellationToken cancellationToken = default)
    {
        var query = GetAll(false);

        // Filtros com OR (qualquer campo preenchido pode corresponder)
        var hasAnyFilter = !string.IsNullOrWhiteSpace(parameters.Name) ||
                          !string.IsNullOrWhiteSpace(parameters.Email) ||
                          !string.IsNullOrWhiteSpace(parameters.Phone);

        if (hasAnyFilter)
        {
            query = query.Where(c =>
                (!string.IsNullOrWhiteSpace(parameters.Name) && 
                 c.Name.ToLower().Contains(parameters.Name!.ToLower())) ||
                (!string.IsNullOrWhiteSpace(parameters.Email) && 
                 c.Email.ToLower().Contains(parameters.Email!.ToLower())) ||
                (!string.IsNullOrWhiteSpace(parameters.Phone) && 
                 c.Phone.ToLower().Contains(parameters.Phone!.ToLower()))
            );
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync(cancellationToken);

        return PaginatedList<Customer>.ToPagedList(items, totalCount, parameters.PageNumber, parameters.PageSize);
    }

    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => GetByCondition(c => c.Id == id, false)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<Customer> UpdateAsync(Customer customer)
    {
        Update(customer);
        return Task.FromResult(customer);
    }
}