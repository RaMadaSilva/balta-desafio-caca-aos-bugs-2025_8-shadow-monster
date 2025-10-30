using BugStore.Application.Features.Customers.CreateCustomer;
using BugStore.Application.Features.Customers.CustomerSearch;
using BugStore.Application.Features.Customers.DeleteCustomer;
using BugStore.Application.Features.Customers.GetByIdCustomer;
using BugStore.Application.Features.Customers.GetCustomers;
using BugStore.Application.Features.Customers.UpdateCustomer;
using BugStore.Application.Features.Orders.CreateOrder;
using BugStore.Application.Features.Orders.GetOrderById;
using BugStore.Application.Features.Orders.OrderSearch;
using BugStore.Application.Features.Products.CreateProduct;
using BugStore.Application.Features.Products.DeleteProduct;
using BugStore.Application.Features.Products.GetProductById;
using BugStore.Application.Features.Products.GetProducts;
using BugStore.Application.Features.Products.ProductSearch;
using BugStore.Application.Features.Products.UpdateProduct;
using BugStore.Application.Features.Reports.BestCustomers;
using BugStore.Application.Features.Reports.RevenueByPeriod;
using BugStore.Domain.Contracts.IRepositories;
using BugStore.Infrastructure.Data;
using BugStore.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;

namespace BugStore.Api.Extensions; 

public static class Registers
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(x => x.UseSqlite(connectionString));
        return services;

    }

    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        // Registrar Repositories (Abstrações)
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Registrar Validators
        services.AddScoped<AbstractValidator<CreateCustomerRequest>, CreateCustomerValidator>();
        services.AddScoped<AbstractValidator<UpdateCustomerRequest>, UpdateCustomerValidator>();
        services.AddScoped<AbstractValidator<DeleteCustomerRequest>, DeleteCustomerValidator>();
        services.AddScoped<AbstractValidator<CreateProductRequest>, CreateProductValidator>();
        services.AddScoped<AbstractValidator<UpdateProductRequest>, UpdateProductValidator>();
        services.AddScoped<AbstractValidator<DeleteProductRequest>, DeleteProductValidator>();
        services.AddScoped<AbstractValidator<CreateOrderRequest>, CreateOrderValidator>();

        // Registrar Handlers - Customers
        services.AddScoped<CreateCustomerHandler>();
        services.AddScoped<UpdateCustomerHandler>();
        services.AddScoped<DeleteCustomerHandler>();
        services.AddScoped<GetCustomersHandler>();
        services.AddScoped<GetByIdCustomerHandler>();
        services.AddScoped<CustomerSearchHandler>();

        // Registrar Handlers - Products
        services.AddScoped<CreateProductHandler>();
        services.AddScoped<UpdateProductHandler>();
        services.AddScoped<DeleteProductHandler>();
        services.AddScoped<GetProductsHandler>();
        services.AddScoped<GetProductByIdHandler>();
        services.AddScoped<ProductSearchHandler>();

        // Registrar Handlers - Orders
        services.AddScoped<CreateOrderHandler>();
        services.AddScoped<GetOrderByIdHandler>();
        services.AddScoped<OrderSearchHandler>();

        // Registrar Handlers - Reports
        services.AddScoped<RevenueByPeriodHandler>();
        services.AddScoped<BestCustomerHandler>();

        return services;
    }

    public static IServiceCollection AddSwaggerDoc(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "BugStore API",
                Version = "v1",
                Description = "API para gerenciamento de loja - Caça aos Bugs 2025",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Name = "BugStore Team"
                }
            });

            c.UseInlineDefinitionsForEnums();
            c.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
        });

        return services;
    }
}
