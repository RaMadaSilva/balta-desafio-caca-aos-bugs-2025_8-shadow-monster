using BugStore.Application.Features.Customers.DeleteCustomer;
using BugStore.Domain.Exceptions;

namespace BugStore.Api.Endpoints.Customers; 

public static class DeleteCustomerEndpoint
{
    public static void MapDeleteCustomerEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/v1/customers/{id:guid}", HandleAsync)
            .WithTags("Customers")
            .WithName("DeleteCustomer")
            .Produces<DeleteCustomerResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }

    static async Task<IResult> HandleAsync(
        Guid id,
        DeleteCustomerHandler handler)
    {
        try
        {
            var request = new DeleteCustomerRequest{Id = id};

            var result = await handler.HandleAsync(request);
            return Results.Ok(result);
        }
        catch (ArgumentException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Results.BadRequest(new { message = ex.Message });
        }
        catch (NotFoundException ex)
        {
            return Results.NotFound(new { message = ex.Message });
        }
    }
}
