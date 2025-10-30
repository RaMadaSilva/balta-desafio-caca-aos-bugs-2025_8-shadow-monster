using BugStore.Domain.Common;
using BugStore.Domain.Contracts.IRepositories;
using BugStore.Domain.Entities;
using BugStore.Infrastructure.Data;
using BugStore.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Infrastructure.Repositories; 

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) 
        : base(context) { }

    public Task<Product> CreateAsync(Product product)
    {
        Add(product); 
        return Task.FromResult(product);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
       var product = await GetByCondition(x=>x.Id==id, true)
            .FirstOrDefaultAsync();

        if(product == null) 
            return false;

        Delete(product);
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
        => await _context.Products.AnyAsync(p => p.Id == id);

    public async Task<PaginatedList<Product>> GetAllAsync(RequestParameters parameters)
    {
        var query = GetAll(false); 

        var count = query.Count();

        var items = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize).ToListAsync();

        return PaginatedList<Product>.ToPagedList(items, count, parameters.PageNumber, parameters.PageSize);
    }

    public Task<Product?> GetByIdAsync(Guid id)
        => GetByCondition(p => p.Id == id, false)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids)
        => await GetByCondition(p => ids.Contains(p.Id), false)
            .ToListAsync();

    public async Task<PaginatedList<Product>> SearchAsync(ProductSearchParameters parameters, CancellationToken cancellationToken = default)
    {
        var query = GetAll(false);

        // Filtros com OR (qualquer campo preenchido pode corresponder)
        var hasAnyFilter = !string.IsNullOrWhiteSpace(parameters.Title) ||
                          !string.IsNullOrWhiteSpace(parameters.Description) ||
                          !string.IsNullOrWhiteSpace(parameters.Slug) ||
                          parameters.Price.HasValue;

        if (hasAnyFilter)
        {
            query = query.Where(p =>
                (!string.IsNullOrWhiteSpace(parameters.Title) && 
                 p.Title.ToLower().Contains(parameters.Title!.ToLower())) ||
                (!string.IsNullOrWhiteSpace(parameters.Description) && 
                 p.Description.ToLower().Contains(parameters.Description!.ToLower())) ||
                (!string.IsNullOrWhiteSpace(parameters.Slug) && 
                 p.Slug.ToLower().Contains(parameters.Slug!.ToLower())) ||
                (parameters.Price.HasValue && p.Price == parameters.Price.Value)
            );
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize).ToListAsync(cancellationToken);

        return PaginatedList<Product>.ToPagedList(items, totalCount, parameters.PageNumber, parameters.PageSize);
    }

    public Task<Product> UpdateAsync(Product product)
    {
       Update(product);
         return  Task.FromResult(product);
    }
}
