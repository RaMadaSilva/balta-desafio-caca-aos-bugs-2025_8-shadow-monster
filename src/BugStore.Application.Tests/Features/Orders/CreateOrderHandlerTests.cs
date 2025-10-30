using BugStore.Application.Features.Orders.CreateOrder;
using BugStore.Domain.Contracts.IRepositories;
using BugStore.Domain.Exceptions;
using FluentAssertions;
using Moq;

namespace BugStore.Application.Tests.Features.Orders.CreateOrder;

public class CreateOrderHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidRequest_ShouldReturnCreatedOrder()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var product1Id = Guid.NewGuid();
        var product2Id = Guid.NewGuid();

        var request = new CreateOrderRequest
        {
            CustomerId = customerId,
            Items = new List<OrderLineItem>
            {
                new OrderLineItem { ProductId = product1Id, Quantity = 2 },
                new OrderLineItem { ProductId = product2Id, Quantity = 1 }
            }
        };

        var customer = new BugStore.Domain.Entities.Customer
        {
            Id = customerId,
            Name = "Test Customer"
        };

        var products = new List<BugStore.Domain.Entities.Product>
        {
            new BugStore.Domain.Entities.Product { Id = product1Id, Title = "Product 1", Price = 10 },
            new BugStore.Domain.Entities.Product { Id = product2Id, Title = "Product 2", Price = 20 }
        };

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var customerRepoMock = new Mock<ICustomerRepository>();
        var productRepoMock = new Mock<IProductRepository>();
        var orderRepoMock = new Mock<IOrderRepository>();
        var validator = new CreateOrderValidator();

        unitOfWorkMock.Setup(x => x.Customers).Returns(customerRepoMock.Object);
        unitOfWorkMock.Setup(x => x.Products).Returns(productRepoMock.Object);
        unitOfWorkMock.Setup(x => x.Orders).Returns(orderRepoMock.Object);
        unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        customerRepoMock.Setup(x => x.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        productRepoMock.Setup(x => x.GetByIdsAsync(It.IsAny<IEnumerable<Guid>>()))
            .ReturnsAsync(products);

        orderRepoMock.Setup(x => x.CreateAsync(It.IsAny<BugStore.Domain.Entities.Order>()))
            .ReturnsAsync((BugStore.Domain.Entities.Order o) => o);

        var handler = new CreateOrderHandler(unitOfWorkMock.Object, validator);

        // Act
        var result = await handler.HandleAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.CustomerId.Should().Be(customerId);
        result.Items.Should().HaveCount(2);
    }

    [Fact]
    public async Task HandleAsync_WithNonExistingCustomer_ShouldThrowArgumentException()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var product1Id = Guid.NewGuid();

        var request = new CreateOrderRequest
        {
            CustomerId = customerId,
            Items = new List<OrderLineItem>
            {
                new OrderLineItem { ProductId = product1Id, Quantity = 1 }
            }
        };

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var customerRepoMock = new Mock<ICustomerRepository>();

        unitOfWorkMock.Setup(x => x.Customers).Returns(customerRepoMock.Object);
        unitOfWorkMock.Setup(x => x.Products).Returns(new Mock<IProductRepository>().Object);
        unitOfWorkMock.Setup(x => x.Orders).Returns(new Mock<IOrderRepository>().Object);

        customerRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((BugStore.Domain.Entities.Customer?)null);

        var validator = new CreateOrderValidator();
        var handler = new CreateOrderHandler(unitOfWorkMock.Object, validator);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            async () => await handler.HandleAsync(request, CancellationToken.None));
    }
}

