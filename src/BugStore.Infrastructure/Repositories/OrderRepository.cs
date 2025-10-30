using BugStore.Domain.Common;
using BugStore.Domain.Contracts.IRepositories;
using BugStore.Domain.DataTransferObject;
using BugStore.Domain.Entities;
using BugStore.Infrastructure.Data;
using BugStore.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Infrastructure.Repositories; 

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) 
        : base(context) { }

    public Task<Order> CreateAsync(Order order)
    {
        Add(order); 
        return Task.FromResult(order);
    }

    public async Task<PaginatedList<BestCustomerDto>> GetBestCustomerAsync(BestCustomerParameters parameters, CancellationToken cancellationToken = default)
    {
        var query = GetAll(false)
            .Include(o => o.Customer)
            .Include(o => o.Lines)
                .ThenInclude(ol => ol.Product)
            .AsQueryable();

        var bestCustomersQuery = query
                            .AsEnumerable()
                            .GroupBy(o => new { o.CustomerId, o.Customer.Name, o.Customer.Email })
                            .Select(g => new BestCustomerDto(g.Key.Name,
                                    g.Key.Email, 
                                    g.Count(),
                                    g.Sum(o => o.Lines.Sum(ol => ol.Quantity * ol.Product.Price))))
                            .OrderByDescending(bc => bc.SpentAmount)
                            .Take(parameters.Top); 

        var count =  bestCustomersQuery.Count();

        var bestCustomers = bestCustomersQuery
                            .Skip((parameters.PageNumber-1)*parameters.PageSize)
                            .Take(parameters.PageSize)
                            .ToList();

        return PaginatedList<BestCustomerDto>.ToPagedList(bestCustomers, count, parameters.PageNumber, parameters.PageSize);
    }

    public Task<Order?> GetByIdAsync(Guid id)
        => GetByCondition(o => o.Id == id, false)
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<Order>> GetOrdersByPeriodAsync(DateTime startPeriod, DateTime endPeriod, CancellationToken cancellationToken)
                                            => await GetAll(false)
                                                .Include(o => o.Customer)
                                                .Include(o => o.Lines)
                                                    .ThenInclude(ol => ol.Product)
                                                .Where(o => o.CreatedAt >= startPeriod && o.CreatedAt <= endPeriod)
                                                .ToListAsync(cancellationToken);

    public async Task<PaginatedList<Order>> SearchAsync(OrderParameters parameters, 
        CancellationToken cancellationToken = default)
    {
        IQueryable<Order> query = GetAll(false)
            .Include(x => x.Customer)
            .Include(x => x.Lines)
                .ThenInclude(x => x.Product);

        // Filtro por Id (exato)
        query = query.WhereIf(parameters.Id != Guid.Empty, x => x.Id == parameters.Id);

        // Filtros de Customer com OR
        var hasCustomerFilter = !string.IsNullOrWhiteSpace(parameters.CustomerName) ||
                               !string.IsNullOrWhiteSpace(parameters.CustomerEmail) ||
                               !string.IsNullOrWhiteSpace(parameters.CustomerPhone);

        if (hasCustomerFilter)
        {
            query = query.Where(o =>
                (!string.IsNullOrWhiteSpace(parameters.CustomerName) && 
                 o.Customer.Name.ToLower().Contains(parameters.CustomerName!.ToLower())) ||
                (!string.IsNullOrWhiteSpace(parameters.CustomerEmail) && 
                 o.Customer.Email.ToLower().Contains(parameters.CustomerEmail!.ToLower())) ||
                (!string.IsNullOrWhiteSpace(parameters.CustomerPhone) && 
                 o.Customer.Phone.ToLower().Contains(parameters.CustomerPhone!.ToLower()))
            );
        }

        // Filtros de Product com OR (buscando em qualquer linha do pedido)
        var hasProductFilter = !string.IsNullOrWhiteSpace(parameters.ProductTitle) ||
                              !string.IsNullOrWhiteSpace(parameters.ProductDescription) ||
                              !string.IsNullOrWhiteSpace(parameters.ProductSlug);

        if (hasProductFilter)
        {
            query = query.Where(o => o.Lines.Any(line =>
                (!string.IsNullOrWhiteSpace(parameters.ProductTitle) && 
                 line.Product.Title.ToLower().Contains(parameters.ProductTitle!.ToLower())) ||
                (!string.IsNullOrWhiteSpace(parameters.ProductDescription) && 
                 line.Product.Description.ToLower().Contains(parameters.ProductDescription!.ToLower())) ||
                (!string.IsNullOrWhiteSpace(parameters.ProductSlug) && 
                 line.Product.Slug.ToLower().Contains(parameters.ProductSlug!.ToLower()))
            ));
        }

        // Filtro por faixa de preço do produto
        if (parameters.ProductPriceStart > 0 || parameters.ProductPriceEnd > 0)
        {
            query = query.Where(o => o.Lines.Any(line =>
                (parameters.ProductPriceStart == 0 || line.Product.Price >= parameters.ProductPriceStart) &&
                (parameters.ProductPriceEnd == 0 || line.Product.Price <= parameters.ProductPriceEnd)
            ));
        }

        // Filtros de data
        query = query.WhereIf(parameters.CreatedAtStart != default, 
            x => x.CreatedAt >= parameters.CreatedAtStart);
        query = query.WhereIf(parameters.CreatedAtEnd != default, 
            x => x.CreatedAt <= parameters.CreatedAtEnd);
        query = query.WhereIf(parameters.UpdatedAtStart != default, 
            x => x.UpdatedAt >= parameters.UpdatedAtStart);
        query = query.WhereIf(parameters.UpdatedAtEnd != default, 
            x => x.UpdatedAt <= parameters.UpdatedAtEnd);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync(cancellationToken);

        return PaginatedList<Order>.ToPagedList(items, totalCount, parameters.PageNumber, parameters.PageSize);
    }
}
