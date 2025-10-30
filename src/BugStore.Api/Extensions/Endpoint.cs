using BugStore.Api.Endpoints.Customers;
using BugStore.Api.Endpoints.Orders;
using BugStore.Api.Endpoints.Products;
using BugStore.Api.Endpoints.Reports;

namespace BugStore.Api.Extensions; 

public static class Endpoint
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", () => "BugStore API - Hello World!");

        // Registrar Endpoints - Customers
        app.MapCreateCustomerEndpoint();
        app.MapUpdateCustomerEndpoint();
        app.MapDeleteCustomerEndpoint();
        app.MapGetCustomersEndpoint();
        app.MapGetByIdCustomerEndpoint();
        app.MapCustomerSearchEndpoint();

        // Registrar Endpoints - Products
        app.MapCreateProductEndpoint();
        app.MapUpdateProductEndpoint();
        app.MapDeleteProductEndpoint();
        app.MapGetProductsEndpoint();
        app.MapGetProductByIdEndpoint();
        app.MapProductSearchEndpoint();

        // Registrar Endpoints - Orders
        app.MapCreateOrderEndpoint();
        app.MapGetOrderByIdEndpoint();
        app.MapOrderSearchEndpoint();

        // Registrar Endpoints - Reports
        app.MapRevenueByPeriodEndpoint();
        app.MapBestCustomersEndpoint();
    }
}
