using BugStore.Application.Features.Products.CreateProduct;
using BugStore.Domain.Contracts.IRepositories;
using FluentAssertions;
using Moq;

namespace BugStore.Application.Tests.Features.Products.CreateProduct;

public class CreateProductHandlerTests
{
    [Fact]
    public async Task HandleAsync_WithValidRequest_ShouldReturnCreatedProduct()
    {
        // Arrange
        var request = new CreateProductRequest
        {
            Title = "Produto Teste",
            Description = "Descrição",
            Slug = "produto-teste",
            Price = 99.99m
        };

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var productRepoMock = new Mock<IProductRepository>();
        var validator = new CreateProductValidator();

        unitOfWorkMock.Setup(x => x.Products).Returns(productRepoMock.Object);
        unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        productRepoMock.Setup(x => x.CreateAsync(It.IsAny<BugStore.Domain.Entities.Product>()))
            .ReturnsAsync((BugStore.Domain.Entities.Product p) => p);

        var handler = new CreateProductHandler(unitOfWorkMock.Object, validator);

        // Act
        var result = await handler.HandleAsync(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title);
        result.Price.Should().Be(request.Price);
    }
}

