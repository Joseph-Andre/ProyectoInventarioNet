using System;
using System.Collections.Generic;
using System.Text;

using LinqAdvancedLab.Data.Models;

namespace LinqAdvancedLab.Tests.Helpers;

/// <summary>
/// Provee datos de prueba para los tests
/// </summary>
public static class TestDataSeeder
{
    public static void SeedProducts(NorthwindContext context)
    {
        var categories = new List<Category>
        {
            new() { CategoryId = 1, CategoryName = "Beverages" },
            new() { CategoryId = 2, CategoryName = "Condiments" },
            new() { CategoryId = 3, CategoryName = "Seafood" }
        };

        var products = new List<Product>
        {
            new() { ProductId = 1, ProductName = "Chai", CategoryId = 1, UnitPrice = 18.00m, UnitsInStock = 39, Discontinued = false },
            new() { ProductId = 2, ProductName = "Chang", CategoryId = 1, UnitPrice = 19.00m, UnitsInStock = 17, Discontinued = false },
            new() { ProductId = 3, ProductName = "Aniseed Syrup", CategoryId = 2, UnitPrice = 10.00m, UnitsInStock = 13, Discontinued = false },
            new() { ProductId = 4, ProductName = "Expensive Wine", CategoryId = 1, UnitPrice = 75.00m, UnitsInStock = 5, Discontinued = false },
            new() { ProductId = 5, ProductName = "Discontinued Item", CategoryId = 2, UnitPrice = 20.00m, UnitsInStock = 0, Discontinued = true },
            new() { ProductId = 6, ProductName = "Salmon", CategoryId = 3, UnitPrice = 55.00m, UnitsInStock = 12, Discontinued = false },
            new() { ProductId = 7, ProductName = "Tuna", CategoryId = 3, UnitPrice = 45.00m, UnitsInStock = 8, Discontinued = false },
            new() { ProductId = 8, ProductName = "Premium Vodka", CategoryId = 1, UnitPrice = 120.00m, UnitsInStock = 3, Discontinued = false },
        };

        context.Categories.AddRange(categories);
        context.Products.AddRange(products);
        context.SaveChanges();
    }

    public static void SeedOrders(NorthwindContext context)
    {
        var customers = new List<Customer>
        {
            new() { CustomerId = "ALFKI", CompanyName = "Alfreds Futterkiste", ContactName = "Maria Anders", City = "Berlin", Country = "Germany" },
            new() { CustomerId = "BONAP", CompanyName = "Bon app'", ContactName = "Laurence Lebihan", City = "Marseille", Country = "France" }
        };

        var employees = new List<Employee>
        {
            new() { EmployeeId = 1, FirstName = "Nancy", LastName = "Davolio", Title = "Sales Representative" },
            new() { EmployeeId = 2, FirstName = "Andrew", LastName = "Fuller", Title = "Vice President" }
        };

        var orders = new List<Order>
        {
            new() { OrderId = 10248, CustomerId = "ALFKI", EmployeeId = 1, OrderDate = DateTime.Now.AddDays(-10), Freight = 32.38m },
            new() { OrderId = 10249, CustomerId = "BONAP", EmployeeId = 2, OrderDate = DateTime.Now.AddDays(-5), Freight = 11.61m },
            new() { OrderId = 10250, CustomerId = "ALFKI", EmployeeId = 1, OrderDate = DateTime.Now.AddDays(-400), Freight = 65.83m }
        };

        var orderDetails = new List<OrderDetail>
        {
            new() { OrderId = 10248, ProductId = 1, UnitPrice = 18.00m, Quantity = 12, Discount = 0 },
            new() { OrderId = 10248, ProductId = 2, UnitPrice = 19.00m, Quantity = 10, Discount = 0 },
            new() { OrderId = 10249, ProductId = 3, UnitPrice = 10.00m, Quantity = 5, Discount = 0.05f }
        };

        context.Customers.AddRange(customers);
        context.Employees.AddRange(employees);
        context.Orders.AddRange(orders);
        context.OrderDetails.AddRange(orderDetails);
        context.SaveChanges();
    }
}