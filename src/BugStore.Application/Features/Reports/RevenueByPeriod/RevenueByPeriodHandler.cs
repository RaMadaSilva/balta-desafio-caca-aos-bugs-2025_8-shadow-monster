using BugStore.Domain.Common;
using BugStore.Domain.Contracts.IRepositories;

namespace BugStore.Application.Features.Reports.RevenueByPeriod; 

public class RevenueByPeriodHandler
{
    private readonly IUnitOfWork _unitOfWork;
    public RevenueByPeriodHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResponse<RevenueByPeriodResponse>> HandleAsync(RevenueByPeriodRequest request, CancellationToken cancellationToken)
    {
         VerifyPeriod(request.StartPeriod, request.EndPeriod);

        var orders = await _unitOfWork.Orders.GetOrdersByPeriodAsync(request.StartPeriod, request.EndPeriod, cancellationToken);
        
        var revenueByPeriod = orders
            .GroupBy(o => new { o.CreatedAt.Year, o.CreatedAt.Month })
            .OrderBy(g => g.Key.Year)
            .ThenBy(g => g.Key.Month)
            .Select(g => new RevenueByPeriodResponse
            {
                Year = g.Key.Year,
                Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture),
                TotalOrders = g.Count(),
                TotalRevenue = g.Sum(o => o.Lines.Sum(x=>x.Quantity*x.Product.Price))
            })
            .ToList();

        var pagedRevenueByPeriod = revenueByPeriod
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();



        return new  PagedResponse<RevenueByPeriodResponse>(pagedRevenueByPeriod, 
                                                            revenueByPeriod.Count, 
                                                            request.PageSize,
                                                            request.PageNumber);
    }

    private void VerifyPeriod(DateTime startPeriod, DateTime endPeriod)
    {
        if(startPeriod > endPeriod)
        {
            throw new ArgumentException("StartPeriod não pode ser posterior a EndPeriod.");
        }
    }
}
