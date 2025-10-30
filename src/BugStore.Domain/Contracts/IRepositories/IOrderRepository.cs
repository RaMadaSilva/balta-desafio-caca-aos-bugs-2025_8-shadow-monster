using BugStore.Domain.Common;
using BugStore.Domain.DataTransferObject;
using BugStore.Domain.Entities;

namespace BugStore.Domain.Contracts.IRepositories; 

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<Order> CreateAsync(Order order);
    Task<IEnumerable<Order>> GetOrdersByPeriodAsync(DateTime startPeriod, DateTime endPeriod, CancellationToken cancellationToken); 

    Task<PaginatedList<Order>> SearchAsync(OrderParameters parameters, CancellationToken cancellationToken = default);
    Task<PaginatedList<BestCustomerDto>> GetBestCustomerAsync(BestCustomerParameters parameters, CancellationToken cancellationToken = default);
}
