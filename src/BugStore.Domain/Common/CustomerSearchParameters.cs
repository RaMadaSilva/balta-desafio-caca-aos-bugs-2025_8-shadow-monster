namespace BugStore.Domain.Common;

public class CustomerSearchParameters : RequestParameters
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}

