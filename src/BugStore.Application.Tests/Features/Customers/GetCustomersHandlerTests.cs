using BugStore.Application.Features.Customers.GetCustomers;
using BugStore.Application.Tests.Helpers;
using BugStore.Domain.Common;
using BugStore.Domain.Contracts.IRepositories;
using FluentAssertions;
using Moq;

namespace BugStore.Application.Tests.Features.Customers.GetCustomers;

public class GetCustomersHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldReturnListOfCustomers()
    {
        // Arrange
        var customers = new List<BugStore.Domain.Entities.Customer>
        {
            TestDataHelper.CreateCustomer(),
            TestDataHelper.CreateCustomer(),
            TestDataHelper.CreateCustomer()
        };

        var paginatedCustomers = new PaginatedList<BugStore.Domain.Entities.Customer>(
            customers, 
            customers.Count, 
            10, 
            1
        );

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var customerRepoMock = new Mock<ICustomerRepository>();

        unitOfWorkMock.Setup(x => x.Customers).Returns(customerRepoMock.Object);
        customerRepoMock.Setup(x => x.GetAllAsync(It.IsAny<RequestParameters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paginatedCustomers);

        var handler = new GetCustomersHandler(unitOfWorkMock.Object);
        var request = new GetCustomersRequest();

        // Act
        var result = await handler.HandleAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(3);
        result.TotalCount.Should().Be(3);
    }

    [Fact]
    public async Task HandleAsync_WithEmptyDatabase_ShouldReturnEmptyList()
    {
        // Arrange
        var emptyPaginatedCustomers = new PaginatedList<BugStore.Domain.Entities.Customer>(
            new List<BugStore.Domain.Entities.Customer>(), 
            0, 
            10, 
            1
        );

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var customerRepoMock = new Mock<ICustomerRepository>();

        unitOfWorkMock.Setup(x => x.Customers).Returns(customerRepoMock.Object);
        customerRepoMock.Setup(x => x.GetAllAsync(It.IsAny<RequestParameters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(emptyPaginatedCustomers);

        var handler = new GetCustomersHandler(unitOfWorkMock.Object);
        var request = new GetCustomersRequest();

        // Act
        var result = await handler.HandleAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }
}

