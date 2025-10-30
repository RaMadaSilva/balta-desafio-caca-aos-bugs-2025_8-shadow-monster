using BugStore.Application.Features.Customers.CustomerSearch;
using BugStore.Application.Features.Customers.GetCustomers;
using BugStore.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace BugStore.Api.Endpoints.Customers;

public static class CustomerSearchEndpoint
{
    public static void MapCustomerSearchEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/customers/search", HandleAsync)
            .WithTags("Customers")
            .WithName("CustomerSearch")
            .Produces<PagedResponse<GetCustomersResponse>>(StatusCodes.Status200OK);
    }

    static async Task<IResult> HandleAsync([AsParameters] CustomerSearchRequest request, 
        [FromServices] CustomerSearchHandler handler)
    {
        var result = await handler.HandleAsync(request);
        return Results.Ok(result);
    }
}
