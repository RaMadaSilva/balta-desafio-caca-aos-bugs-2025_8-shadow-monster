using BugStore.Domain.Entities;
using FluentAssertions;

namespace BugStore.Domain.Tests.Entities;

public class CustomerTests
{
    [Fact]
    public void Customer_Should_HaveId()
    {
        // Arrange
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Email = "test@email.com"
        };

        // Act & Assert
        customer.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Customer_Should_HaveRequiredProperties()
    {
        // Arrange & Act
        var customer = new Customer
        {
            Name = "João Silva",
            Email = "joao@email.com",
            Phone = "11999999999",
            BirthDate = DateTime.Now
        };

        // Assert
        customer.Name.Should().NotBeNullOrEmpty();
        customer.Email.Should().NotBeNullOrEmpty();
        customer.Phone.Should().NotBeNullOrEmpty();
        customer.BirthDate.Should().BeAfter(DateTime.MinValue);
    }

    [Fact]
    public void Customer_Should_BeInstanceOfBaseEntity()
    {
        // Arrange & Act
        var customer = new Customer();

        // Assert
        customer.Should().BeAssignableTo<BaseEntity>();
    }
}

