using BugStore.Application.Features.Customers.CreateCustomer;

namespace BugStore.Api.Endpoints.Customers; 

public static class CreateCustomerEndpoint
{
    public static void MapCreateCustomerEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/v1/customers", HandleAsync)
            .WithTags("Customers")
            .WithName("CreateCustomer")
            .Produces<CreateCustomerResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
    }

    static async Task<IResult> HandleAsync(
        CreateCustomerRequest request,
        CreateCustomerHandler handler, CancellationToken cancellationToken)
    {
        try
        {
            var result = await handler.HandleAsync(request, cancellationToken);
            return Results.Created($"/v1/customers/{result.Id}", result);
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
