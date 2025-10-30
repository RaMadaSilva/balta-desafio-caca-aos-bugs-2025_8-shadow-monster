using BugStore.Application.Features.Reports.BestCustomers;
using BugStore.Application.Mappings.Reports;
using BugStore.Domain.Common;
using BugStore.Domain.Contracts.IRepositories;
using BugStore.Domain.DataTransferObject;
using FluentAssertions;
using Moq;

namespace BugStore.Application.Tests.Features.Reports;

public class BestCustomerHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Map_Dto_To_Response_And_Pagination()
    {
        var dto = new PaginatedList<BestCustomerDto>(
            new List<BestCustomerDto>
            {
                new BestCustomerDto("Alice","alice@mail.com", 3, 150m),
                new BestCustomerDto("Bob","bob@mail.com", 2, 120m)
            },
            totalCount: 5,
            pageSize: 2,
            currentPage: 1);

        var uowMock = new Mock<IUnitOfWork>();
        var orderRepoMock = new Mock<IOrderRepository>();
        orderRepoMock
            .Setup(r => r.GetBestCustomerAsync(It.IsAny<BestCustomerParameters>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);
        uowMock.Setup(x => x.Orders).Returns(orderRepoMock.Object);

        var handler = new BestCustomerHandler(uowMock.Object);
        var result = await handler.HandleAsync(new BestCustomersRequest
        {
            Top = 10,
            PageNumber = 1,
            PageSize = 2
        }, CancellationToken.None);

        result.TotalCount.Should().Be(5);
        result.PageSize.Should().Be(2);
        result.CurrentPage.Should().Be(1);
        result.Items.Should().HaveCount(2);
        result.Items.First().CustomerName.Should().Be("Alice");
        result.Items.First().SpentAmount.Should().Be(150m);
    }
}


