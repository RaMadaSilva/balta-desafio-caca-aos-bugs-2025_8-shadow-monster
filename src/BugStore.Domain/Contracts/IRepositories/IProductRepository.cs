using BugStore.Domain.Common;
using BugStore.Domain.Entities;

namespace BugStore.Domain.Contracts.IRepositories; 

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<PaginatedList<Product>> GetAllAsync(RequestParameters parameters);
    Task<PaginatedList<Product>> SearchAsync(ProductSearchParameters parameters, CancellationToken cancellationToken = default);
    Task<Product> CreateAsync(Product product);
    Task<Product> UpdateAsync(Product product);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<Guid> ids);
}
