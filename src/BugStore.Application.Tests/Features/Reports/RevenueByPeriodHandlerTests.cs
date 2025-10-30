using BugStore.Application.Features.Reports.RevenueByPeriod;
using BugStore.Domain.Contracts.IRepositories;
using BugStore.Domain.Entities;
using FluentAssertions;
using Moq;

namespace BugStore.Application.Tests.Features.Reports;

public class RevenueByPeriodHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Aggregate_By_Year_And_Month_And_Sort()
    {
        var orders = new List<Order>
        {
            new Order
            {
                CreatedAt = new DateTime(2025, 1, 10),
                Lines = new List<OrderLine>
                {
                    new OrderLine { Quantity = 2, Product = new Product { Price = 10m } },
                    new OrderLine { Quantity = 1, Product = new Product { Price = 5m } }
                }
            },
            new Order
            {
                CreatedAt = new DateTime(2025, 1, 20),
                Lines = new List<OrderLine>
                {
                    new OrderLine { Quantity = 3, Product = new Product { Price = 7m } }
                }
            },
            new Order
            {
                CreatedAt = new DateTime(2025, 2, 5),
                Lines = new List<OrderLine>
                {
                    new OrderLine { Quantity = 1, Product = new Product { Price = 100m } }
                }
            }
        };

        var uowMock = new Mock<IUnitOfWork>();
        var orderRepoMock = new Mock<IOrderRepository>();
        orderRepoMock
            .Setup(r => r.GetOrdersByPeriodAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(orders);

        uowMock.Setup(x => x.Orders).Returns(orderRepoMock.Object);

        var handler = new RevenueByPeriodHandler(uowMock.Object);
        var request = new RevenueByPeriodRequest
        {
            StartPeriod = new DateTime(2025, 1, 1),
            EndPeriod = new DateTime(2025, 12, 31),
            PageNumber = 1,
            PageSize = 10
        };

        var result = await handler.HandleAsync(request, CancellationToken.None);

        result.TotalCount.Should().Be(2);
        result.Items.Should().HaveCount(2);

        var jan = result.Items.First();
        jan.Year.Should().Be(2025);
        jan.TotalOrders.Should().Be(2);
        jan.TotalRevenue.Should().Be(2*10 + 1*5 + 3*7);

        var feb = result.Items.Skip(1).First();
        feb.Year.Should().Be(2025);
        feb.TotalOrders.Should().Be(1);
        feb.TotalRevenue.Should().Be(100);
    }

    [Fact]
    public async Task HandleAsync_Should_Throw_When_Start_Greater_Than_End()
    {
        var uowMock = new Mock<IUnitOfWork>();
        var handler = new RevenueByPeriodHandler(uowMock.Object);

        var request = new RevenueByPeriodRequest
        {
            StartPeriod = new DateTime(2025, 2, 1),
            EndPeriod = new DateTime(2025, 1, 1),
            PageNumber = 1,
            PageSize = 10
        };

        var act = async () => await handler.HandleAsync(request, CancellationToken.None);
        await act.Should().ThrowAsync<ArgumentException>();
    }
}


