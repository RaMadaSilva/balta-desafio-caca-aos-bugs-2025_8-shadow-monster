using BugStore.Application.Features.Customers.CreateCustomer;
using BugStore.Domain.Contracts.IRepositories;
using FluentAssertions;
using Moq;

namespace BugStore.Application.Tests.Features.Customers.CreateCustomer;

public class CreateCustomerHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidRequest_ShouldReturnCreatedCustomer()
    {
        // Arrange
        var request = new CreateCustomerRequest
        {
            Name = "João Silva",
            Email = "joao@email.com",
            Phone = "11999999999",
            BirthDate = new DateTime(1990, 1, 1)
        };

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var customerRepoMock = new Mock<ICustomerRepository>();
        var validator = new CreateCustomerValidator();

        unitOfWorkMock.Setup(x => x.Customers).Returns(customerRepoMock.Object);
        unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        customerRepoMock.Setup(x => x.CreateAsync(It.IsAny<BugStore.Domain.Entities.Customer>()))
            .ReturnsAsync((BugStore.Domain.Entities.Customer c) => c);

        var handler = new CreateCustomerHandler(unitOfWorkMock.Object, validator);

        // Act
        var result = await handler.HandleAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Email.Should().Be(request.Email);
    }
}

