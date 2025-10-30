using BugStore.Application.Features.Products.CreateProduct;

namespace BugStore.Api.Endpoints.Products; 

public static class CreateProductEndpoint
{
    public static void MapCreateProductEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/v1/products", HandleAsync)
            .WithTags("Products")
            .WithName("CreateProduct")
            .Produces<CreateProductResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
    }

    static async Task<IResult> HandleAsync(
        CreateProductRequest request,
        CreateProductHandler handler, 
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await handler.HandleAsync(request, cancellationToken);
            return Results.Created($"/v1/products/{result.Id}", result);
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


