using LinqAdvancedLab.Data.Models;
using LinqAdvancedLab.Data.Specifications.Products; // ← Cambiado
using LinqAdvancedLab.Data.Specifications.Orders;   // ← Cambiado
using LinqAdvancedLab.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LinqAdvancedLab.Console;

/// <summary>
/// Runner para demostrar el uso del patrón Repository + Specification
/// </summary>
public class SpecificationQueryRunner
{
    private readonly IRepository<Product> _productRepository;
    private readonly IRepository<Order> _orderRepository;

    public SpecificationQueryRunner(
        IRepository<Product> productRepository,
        IRepository<Order> orderRepository)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
    }

    public async Task RunAsync()
    {
        System.Console.WriteLine("\n" + "=".PadRight(80, '='));
        System.Console.WriteLine("SPECIFICATION PATTERN - Queries Refactorizadas");
        System.Console.WriteLine("=".PadRight(80, '='));
        System.Console.WriteLine();

        await DemoProductsWithCategory();
        await DemoExpensiveProducts();
        await DemoPaginatedProducts();
        await DemoDiscontinuedProducts();
        await DemoProductsByCategory();
        await DemoOrdersWithDetails();
        await DemoRecentOrders();

        System.Console.WriteLine("\n" + "=".PadRight(80, '='));
        System.Console.WriteLine("✅ Demostración completada");
        System.Console.WriteLine("=".PadRight(80, '='));
    }

    // ========== DEMO 1: Productos con Categoría ==========
    private async Task DemoProductsWithCategory()
    {
        System.Console.WriteLine("📋 SPEC 1: Productos con Categoría (Include)");
        System.Console.WriteLine("-".PadRight(80, '-'));

        var spec = new ProductsWithCategorySpecification();
        var products = await _productRepository.GetAsync(spec);

        System.Console.WriteLine($"   Total productos: {products.Count}");
        System.Console.WriteLine($"   Primeros 5:");
        foreach (var p in products.Take(5))
        {
            System.Console.WriteLine($"     • [{p.ProductId}] {p.ProductName}");
            System.Console.WriteLine($"       Categoría: {p.Category?.CategoryName ?? "N/A"}");
        }
        System.Console.WriteLine();
    }

    // ========== DEMO 2: Productos caros ==========
    private async Task DemoExpensiveProducts()
    {
        System.Console.WriteLine("📋 SPEC 2: Productos caros (> $50) con paginación");
        System.Console.WriteLine("-".PadRight(80, '-'));

        var spec = new ExpensiveProductsSpecification(minPrice: 50m, pageIndex: 0, pageSize: 5);
        var products = await _productRepository.GetAsync(spec);
        var totalCount = await _productRepository.CountAsync(new ExpensiveProductsSpecification(50m));

        System.Console.WriteLine($"   Total productos > $50: {totalCount}");
        System.Console.WriteLine($"   Mostrando página 1 (5 items):");
        foreach (var p in products)
        {
            System.Console.WriteLine($"     • [{p.ProductId}] {p.ProductName} - ${p.UnitPrice:F2}");
        }
        System.Console.WriteLine();
    }

    // ========== DEMO 3: Paginación simple ==========
    private async Task DemoPaginatedProducts()
    {
        System.Console.WriteLine("📋 SPEC 3: Productos paginados (página 2)");
        System.Console.WriteLine("-".PadRight(80, '-'));

        int pageSize = 10;
        int pageIndex = 1; // Página 2

        var spec = new ProductsPaginatedSpecification(pageIndex, pageSize);
        var products = await _productRepository.GetAsync(spec);
        var totalCount = await _productRepository.CountAsync(new ProductsPaginatedSpecification(0, int.MaxValue));

        System.Console.WriteLine($"   Total productos: {totalCount}");
        System.Console.WriteLine($"   Página {pageIndex + 1}, mostrando {products.Count} items:");
        foreach (var p in products)
        {
            System.Console.WriteLine($"     • [{p.ProductId}] {p.ProductName}");
        }
        System.Console.WriteLine();
    }

    // ========== DEMO 4: Productos descontinuados ==========
    private async Task DemoDiscontinuedProducts()
    {
        System.Console.WriteLine("📋 SPEC 4: Productos descontinuados");
        System.Console.WriteLine("-".PadRight(80, '-'));

        var spec = new DiscontinuedProductsSpecification();
        var products = await _productRepository.GetAsync(spec);

        System.Console.WriteLine($"   Total descontinuados: {products.Count}");
        foreach (var p in products.Take(5))
        {
            System.Console.WriteLine($"     • [{p.ProductId}] {p.ProductName}");
        }
        System.Console.WriteLine();
    }

    // ========== DEMO 5: Productos por categoría ==========
    private async Task DemoProductsByCategory()
    {
        System.Console.WriteLine("📋 SPEC 5: Productos por categoría (Beverages - ID 1)");
        System.Console.WriteLine("-".PadRight(80, '-'));

        var spec = new ProductsByCategorySpecification(categoryId: 1);
        var products = await _productRepository.GetAsync(spec);

        System.Console.WriteLine($"   Total productos en categoría: {products.Count}");
        foreach (var p in products)
        {
            System.Console.WriteLine($"     • [{p.ProductId}] {p.ProductName}");
            System.Console.WriteLine($"       Categoría: {p.Category?.CategoryName}");
        }
        System.Console.WriteLine();
    }

    // ========== DEMO 6: Órdenes con detalles ==========
    private async Task DemoOrdersWithDetails()
    {
        System.Console.WriteLine("📋 SPEC 6: Órdenes con detalles completos (Include múltiple)");
        System.Console.WriteLine("-".PadRight(80, '-'));

        var spec = new OrdersWithDetailsSpecification();
        var orders = await _orderRepository.GetAsync(spec);

        System.Console.WriteLine($"   Total órdenes: {orders.Count}");
        System.Console.WriteLine($"   Primeras 3:");
        foreach (var o in orders.Take(3))
        {
            System.Console.WriteLine($"     • Orden #{o.OrderId}");
            System.Console.WriteLine($"       Cliente: {o.Customer?.CompanyName ?? "N/A"}");
            System.Console.WriteLine($"       Empleado: {o.Employee?.FirstName} {o.Employee?.LastName}");
            System.Console.WriteLine($"       Fecha: {o.OrderDate:yyyy-MM-dd}");
            System.Console.WriteLine($"       Items: {o.OrderDetails.Count}");
        }
        System.Console.WriteLine();
    }

    // ========== DEMO 7: Órdenes recientes ==========
    private async Task DemoRecentOrders()
    {
        System.Console.WriteLine("📋 SPEC 7: Órdenes recientes (últimos 365 días)");
        System.Console.WriteLine("-".PadRight(80, '-'));

        var spec = new RecentOrdersSpecification(days: 365, pageIndex: 0, pageSize: 5);
        var orders = await _orderRepository.GetAsync(spec);
        var totalCount = await _orderRepository.CountAsync(new RecentOrdersSpecification(365));

        System.Console.WriteLine($"   Total órdenes recientes: {totalCount}");
        System.Console.WriteLine($"   Mostrando primeras 5:");
        foreach (var o in orders)
        {
            System.Console.WriteLine($"     • Orden #{o.OrderId} - {o.OrderDate:yyyy-MM-dd}");
            System.Console.WriteLine($"       Cliente: {o.Customer?.CompanyName}");
        }
        System.Console.WriteLine();
    }
}