using BugStore.Infrastructure.Data;
using BugStore.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BugStore.Infrastructure.Tests.Repositories;

public class CustomerRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly CustomerRepository _repository;

    public CustomerRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new CustomerRepository(_context);
    }

    [Fact]
    public async Task CreateAsync_Should_AddCustomerToDatabase()
    {
        // Arrange
        var customer = new BugStore.Domain.Entities.Customer
        {
            Name = "Test Customer",
            Email = "test@email.com",
            Phone = "11999999999",
            BirthDate = DateTime.Now
        };

        // Act
        await _repository.CreateAsync(customer);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Customers.FindAsync(customer.Id);
        result.Should().NotBeNull();
        result!.Name.Should().Be("Test Customer");
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_Should_ReturnCustomer()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new BugStore.Domain.Entities.Customer
        {
            Id = customerId,
            Name = "Test",
            Email = "test@email.com",
            Phone = "11999999999"
        };

        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(customerId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(customerId);
    }

    [Fact]
    public async Task DeleteAsync_WithExistingCustomer_Should_ReturnTrue()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var customer = new BugStore.Domain.Entities.Customer
        {
            Id = customerId,
            Name = "Test",
            Email = "test@email.com",
            Phone = "11999999999"
        };

        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.DeleteAsync(customerId);
        await _context.SaveChangesAsync();

        // Assert
        result.Should().BeTrue();
        var deletedCustomer = await _context.Customers.FindAsync(customerId);
        deletedCustomer.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingCustomer_Should_ReturnFalse()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _repository.DeleteAsync(nonExistingId);

        // Assert
        result.Should().BeFalse();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

