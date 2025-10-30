using BugStore.Application.Features.Products.GetProductById;

namespace BugStore.Api.Endpoints.Products;

public static class GetProductByIdEndpoint
{
    public static void MapGetProductByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/products/{id}", HandleAsync)
            .WithTags("Products")
            .WithName("GetProductById")
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }

    static async Task<IResult> HandleAsync(
        Guid id,
        GetProductByIdHandler handler)
    {
        var query = new GetProductByIdQuery { Id = id };
        var result = await handler.HandleAsync(query);
        
        if (result == null) 
            return Results.NotFound(new { message = "Product não encontrado" });
        
        return Results.Ok(result);
    }
}


