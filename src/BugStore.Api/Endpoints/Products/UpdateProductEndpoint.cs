using BugStore.Application.Features.Products.UpdateProduct;
using BugStore.Domain.Exceptions;

namespace BugStore.Api.Endpoints.Products;

public static class UpdateProductEndpoint
{
    public static void MapUpdateProductEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPut("/v1/products/{id}", HandleAsync)
            .WithTags("Products")
            .WithName("UpdateProduct")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);
    }

    static async Task<IResult> HandleAsync(
        Guid id,
        UpdateProductRequest request,
        UpdateProductHandler handler, 
        CancellationToken cancellationToken)
    {
        try
        {
            request.Id = id;
            var result = await handler.HandleAsync(request, cancellationToken);
            return Results.Ok(result);
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
    }
}


