using BugStore.Domain.Contracts.IRepositories;
using BugStore.Infrastructure.Data;

namespace BugStore.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private AppDbContext _context; 

    private readonly Lazy<ICustomerRepository> _customerRepository;
    private readonly Lazy<IOrderRepository> _orderRepository;
    private readonly Lazy<IProductRepository> _productRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;

        _customerRepository = new Lazy<ICustomerRepository>(new CustomerRepository(_context));   
        _orderRepository = new Lazy<IOrderRepository>(new OrderRepository(_context));
        _productRepository = new Lazy<IProductRepository>(new ProductRepository(_context));
    }
    public ICustomerRepository Customers => _customerRepository.Value;
    public IOrderRepository Orders => _orderRepository.Value;
    public IProductRepository Products => _productRepository.Value;

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
     => await _context.SaveChangesAsync(cancellationToken) > 0;
}