namespace BugStore.Domain.DataTransferObject; 

public record BestCustomerDto(string CustomerName,  
    string CustomerEmail, 
    int TotalOrders,
    decimal SpentAmount); 
