using BugStore.Application.Features.Products.DeleteProduct;
using BugStore.Domain.Exceptions;

namespace BugStore.Api.Endpoints.Products; 

public static class DeleteProductEndpoint
{
    public static void MapDeleteProductEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/v1/products/{id:guid}", HandleAsync)
            .WithTags("Products")
            .WithName("DeleteProduct")
            .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }

    static async Task<IResult> HandleAsync(
        Guid id,
        DeleteProductHandler handler, 
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new DeleteProductRequest{Id = id};

            var result = await handler.HandleAsync(request, cancellationToken);
            return Results.Ok(result);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(new { message = ex.Message });
        }
    }
}


