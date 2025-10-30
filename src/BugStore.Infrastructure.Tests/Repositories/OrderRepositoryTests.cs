using BugStore.Domain.Common;
using BugStore.Domain.Entities;
using BugStore.Infrastructure.Data;
using BugStore.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Infrastructure.Tests.Repositories;

public class OrderRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly OrderRepository _repository;

    public OrderRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new OrderRepository(_context);
    }

    [Fact]
    public async Task GetOrdersByPeriodAsync_Should_Return_Inclusive_Bounds()
    {
        var customer = new Customer { Name = "C1", Email = "c1@mail.com", Phone = "1" };
        var product = new Product { Title = "P", Description = "D", Slug = "p", Price = 10m };

        var o1 = new Order { Customer = customer, CreatedAt = new DateTime(2025, 1, 1) };
        o1.Lines = new List<OrderLine> { new OrderLine { Product = product, Quantity = 1 } };

        var o2 = new Order { Customer = customer, CreatedAt = new DateTime(2025, 1, 15) };
        o2.Lines = new List<OrderLine> { new OrderLine { Product = product, Quantity = 2 } };

        var o3 = new Order { Customer = customer, CreatedAt = new DateTime(2025, 1, 31) };
        o3.Lines = new List<OrderLine> { new OrderLine { Product = product, Quantity = 3 } };

        _context.Orders.AddRange(o1, o2, o3);
        await _context.SaveChangesAsync();

        var result = await _repository.GetOrdersByPeriodAsync(
            new DateTime(2025, 1, 1), new DateTime(2025, 1, 31), CancellationToken.None);

        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetBestCustomerAsync_Should_Group_Order_And_OrderBy_SpentAmount_And_Top()
    {
        var c1 = new Customer { Name = "Alice", Email = "a@mail.com", Phone = "1" };
        var c2 = new Customer { Name = "Bob", Email = "b@mail.com", Phone = "2" };
        var p = new Product { Title = "P", Description = "D", Slug = "p", Price = 10m };

        var o1 = new Order { Customer = c1, CreatedAt = DateTime.UtcNow };
        o1.Lines = new List<OrderLine> { new OrderLine { Product = p, Quantity = 5 } }; // 50

        var o2 = new Order { Customer = c2, CreatedAt = DateTime.UtcNow };
        o2.Lines = new List<OrderLine> { new OrderLine { Product = p, Quantity = 3 } }; // 30

        var o3 = new Order { Customer = c1, CreatedAt = DateTime.UtcNow };
        o3.Lines = new List<OrderLine> { new OrderLine { Product = p, Quantity = 2 } }; // +20 => 70

        _context.Orders.AddRange(o1, o2, o3);
        await _context.SaveChangesAsync();

        var paged = await _repository.GetBestCustomerAsync(new BestCustomerParameters
        {
            Top = 1,
            PageNumber = 1,
            PageSize = 10
        }, CancellationToken.None);

        paged.TotalCount.Should().Be(1);
        paged.Items.Should().HaveCount(1);
        var first = paged.Items.First();
        first.CustomerName.Should().Be("Alice");
        first.SpentAmount.Should().Be(70m);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}


