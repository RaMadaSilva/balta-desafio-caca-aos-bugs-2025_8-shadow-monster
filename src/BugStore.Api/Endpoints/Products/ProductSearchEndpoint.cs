using BugStore.Application.Features.Products.GetProducts;
using BugStore.Application.Features.Products.ProductSearch;
using BugStore.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace BugStore.Api.Endpoints.Products; 

public static class ProductSearchEndpoint
{
    public static void MapProductSearchEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/product/search", HandleAsync)
            .WithTags("Products")
            .WithName("ProductSearch")
            .Produces<PagedResponse<GetProductsResponse>>(StatusCodes.Status200OK);
    }

    static async Task<IResult> HandleAsync([AsParameters] ProductSearchRequest request,
        [FromServices] ProductSearchHandler handler, CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(request, cancellationToken);
        return Results.Ok(result);
    }
}
