using BugStore.Application.Features.Reports.BestCustomers;
using BugStore.Application.Features.Reports.RevenueByPeriod;
using BugStore.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace BugStore.Api.Endpoints.Reports; 

public static  class BestCustomersEndpoint
{
    public static void MapBestCustomersEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/reports/best-customers", HandleAsync)
             .WithTags("Reports")
             .WithName("GetBestCustomers")
             .Produces<PagedResponse<BestCustomersResponse>>(StatusCodes.Status200OK)
             .Produces(StatusCodes.Status400BadRequest);
    }

    static async Task<IResult> HandleAsync([AsParameters] BestCustomersRequest request,
        [FromServices] BestCustomerHandler handler, 
        CancellationToken cancellationToken)
    {
      var result = await handler.HandleAsync(request, cancellationToken); 
      return Results.Ok(result);
    }
}
