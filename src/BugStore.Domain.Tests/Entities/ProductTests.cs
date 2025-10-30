using BugStore.Domain.Entities;
using FluentAssertions;

namespace BugStore.Domain.Tests.Entities;

public class ProductTests
{
    [Fact]
    public void Product_Should_InitializeWithValidData()
    {
        // Arrange
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Title = "Produto Teste",
            Description = "Descrição",
            Slug = "produto-teste",
            Price = 99.99m
        };

        // Act & Assert
        product.Title.Should().NotBeNullOrEmpty();
        product.Slug.Should().NotBeNullOrEmpty();
        product.Price.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Product_Should_BeInstanceOfBaseEntity()
    {
        // Arrange & Act
        var product = new Product();

        // Assert
        product.Should().BeAssignableTo<BaseEntity>();
    }

    [Fact]
    public void Product_Should_AllowZeroPrice()
    {
        // Arrange & Act
        var product = new Product
        {
            Price = 0m
        };

        // Assert
        product.Price.Should().Be(0);
    }
}

