namespace BugStore.Application.Features.Customers.DeleteCustomer;

public class DeleteCustomerResponse
{
    public Guid Id { get; set; }
    public string Message { get; set; } = "Customer deleted successfully";
}
