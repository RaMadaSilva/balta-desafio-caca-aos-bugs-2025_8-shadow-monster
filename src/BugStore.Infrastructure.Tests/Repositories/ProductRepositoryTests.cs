using BugStore.Domain.Common;
using BugStore.Infrastructure.Data;
using BugStore.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Infrastructure.Tests.Repositories;

internal class TestRequestParameters : RequestParameters { }

public class ProductRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly ProductRepository _repository;

    public ProductRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new ProductRepository(_context);
    }

    [Fact]
    public async Task CreateAsync_Should_AddProductToDatabase()
    {
        // Arrange
        var product = new BugStore.Domain.Entities.Product
        {
            Title = "Test Product",
            Description = "Test Description",
            Slug = "test-product",
            Price = 99.99m
        };

        // Act
        await _repository.CreateAsync(product);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Products.FindAsync(product.Id);
        result.Should().NotBeNull();
        result!.Title.Should().Be("Test Product");
    }

    [Fact]
    public async Task GetAllAsync_Should_ReturnAllProducts()
    {
        // Arrange
        var products = new List<BugStore.Domain.Entities.Product>
        {
            new BugStore.Domain.Entities.Product { Title = "Product 1", Description = "Desc 1", Slug = "product-1", Price = 10 },
            new BugStore.Domain.Entities.Product { Title = "Product 2", Description = "Desc 2", Slug = "product-2", Price = 20 },
            new BugStore.Domain.Entities.Product { Title = "Product 3", Description = "Desc 3", Slug = "product-3", Price = 30 }
        };

        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        var parameters = new TestRequestParameters
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var result = await _repository.GetAllAsync(parameters);

        // Assert
        result.Items.Should().HaveCount(3);
        result.TotalCount.Should().Be(3);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

