namespace BugStore.Domain.Contracts.IRepositories; 

public interface IUnitOfWork
{
    ICustomerRepository Customers { get; }
    IOrderRepository Orders { get; }
    IProductRepository Products { get; }
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
}
