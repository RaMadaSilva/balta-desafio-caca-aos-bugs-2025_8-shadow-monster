using BugStore.Application.Features.Orders.OrderSearch;
using BugStore.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace BugStore.Api.Endpoints.Orders;

public static class OrderSearchEndpoint
{
    public static void MapOrderSearchEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/orders/search", HandleAsync)
            .WithTags("Orders")
            .WithName("OrderSearch")
            .Produces<PagedResponse<OrderSearchResponse>>(StatusCodes.Status200OK);
    }

    static async Task<IResult> HandleAsync([AsParameters] OrderSearchRequest request, 
        [FromServices] OrderSearchHandler handler)
    {
        var result = await handler.HandleAsync(request);
        return Results.Ok(result);
    }
}

