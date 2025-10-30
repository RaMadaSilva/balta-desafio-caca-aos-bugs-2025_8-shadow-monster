using BugStore.Domain.Entities;

namespace BugStore.Application.Tests.Helpers;

public static class TestDataHelper
{
    public static Customer CreateCustomer(Guid? id = null, string? name = null)
    {
        return new Customer
        {
            Id = id ?? Guid.NewGuid(),
            Name = name ?? "João Silva",
            Email = "joao@email.com",
            Phone = "11999999999",
            BirthDate = new DateTime(1990, 1, 1)
        };
    }

    public static Product CreateProduct(Guid? id = null, decimal? price = null)
    {
        return new Product
        {
            Id = id ?? Guid.NewGuid(),
            Title = "Produto Teste",
            Description = "Descrição do produto",
            Slug = "produto-teste",
            Price = price ?? 99.99m
        };
    }

    public static Order CreateOrder(Guid? id = null, Guid? customerId = null)
    {
        return new Order
        {
            Id = id ?? Guid.NewGuid(),
            CustomerId = customerId ?? Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Lines = new List<OrderLine>()
        };
    }
}

