using BugStore.Application.Features.Customers.GetCustomers;

namespace BugStore.Api.Endpoints.Customers;

public static class GetCustomersEndpoint
{
    public static void MapGetCustomersEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/customers", HandleAsync)
            .WithTags("Customers")
            .WithName("GetCustomers")
            .Produces<List<GetCustomersResponse>>(StatusCodes.Status200OK);
    }

    static async Task<IResult> HandleAsync([AsParameters]GetCustomersRequest request, GetCustomersHandler handler)
    {
        var result = await handler.HandleAsync(request);
        return Results.Ok(result);
    }
}