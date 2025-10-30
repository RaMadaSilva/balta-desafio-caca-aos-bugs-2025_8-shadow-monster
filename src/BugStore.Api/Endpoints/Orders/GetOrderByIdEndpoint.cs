using BugStore.Application.Features.Orders.GetOrderById;

namespace BugStore.Api.Endpoints.Orders;

public static class GetOrderByIdEndpoint
{
    public static void MapGetOrderByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/orders/{id}", HandleAsync)
            .WithTags("Orders")
            .WithName("GetOrderById")
            .Produces<GetOrderByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }

    static async Task<IResult> HandleAsync(
        Guid id,
        GetOrderByIdHandler handler)
    {
        var request = new GetOrderByIdRequest { Id = id };
        var result = await handler.HandleAsync(request);
        
        if (result == null) 
            return Results.NotFound(new { message = "Order não encontrada" });
        
        return Results.Ok(result);
    }
}


