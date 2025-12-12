using LinqAdvancedLab.Data;
using LinqAdvancedLab.Data.Models;
using LinqAdvancedLab.Domain.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqAdvancedLab.Console;

public class AdvancedQueryRunner
{
    private readonly NorthwindContext _db;

    // ✅ Expresiones compiladas (Query 8) - Se compilan una sola vez
    private static readonly Func<NorthwindContext, int, int, IEnumerable<Product>> CompiledProductsPaged =
        EF.CompileQuery((NorthwindContext ctx, int skip, int take) =>
            ctx.Products
                .OrderBy(p => p.ProductId)
                .Skip(skip)
                .Take(take));

    // 👇 CORREGIDO: Incluir Take dentro de la expresión compilada
    private static readonly Func<NorthwindContext, decimal, int, IEnumerable<Product>> CompiledExpensiveProducts =
        EF.CompileQuery((NorthwindContext ctx, decimal minPrice, int take) =>
            ctx.Products
                .Where(p => p.UnitPrice >= minPrice)
                .OrderByDescending(p => p.UnitPrice)
                .Take(take));  // ← Take dentro de la expresión compilada

    public AdvancedQueryRunner(NorthwindContext db)
    {
        _db = db;
    }

    public async Task RunAsync()
    {
        System.Console.WriteLine("\n" + "=".PadRight(80, '='));
        System.Console.WriteLine("LINQ ADVANCED LAB - Queries Avanzadas 5-8");
        System.Console.WriteLine("=".PadRight(80, '='));
        System.Console.WriteLine();

        System.Console.WriteLine("Comprobando conexión a la BD...");
        if (!await _db.Database.CanConnectAsync())
        {
            System.Console.WriteLine("❌ No se puede conectar a la base de datos.");
            return;
        }
        System.Console.WriteLine("✅ Conexión exitosa\n");

        var sb = new StringBuilder();

        try
        {
            // Ejecutar todas las queries avanzadas
            await RunQuery5Pagination(sb);
            await RunQuery6PaginationAdvanced(sb);
            await RunQuery7Union(sb);
            await RunQuery8CompiledExpressions(sb);

            // Guardar SQLs en docs/
            var docsDir = @"C:\Users\PC\source\repos\Joseph-Andre\ProyectoInventarioNet\docs";
            Directory.CreateDirectory(docsDir);
            var outPath = Path.Combine(docsDir, "queries_5_to_8.sql");
            await File.WriteAllTextAsync(outPath, sb.ToString(), Encoding.UTF8);

            System.Console.WriteLine("\n" + "=".PadRight(80, '='));
            System.Console.WriteLine($"✅ SQL guardado en: {outPath}");
            System.Console.WriteLine("=".PadRight(80, '='));
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"\n❌ ERROR: {ex.Message}");
            System.Console.WriteLine($"StackTrace: {ex.StackTrace}");
            throw;
        }
    }

    // ========== QUERY 5: Paginación básica con Skip/Take ==========
    private async Task RunQuery5Pagination(StringBuilder sb)
    {
        System.Console.WriteLine("📋 QUERY 5: Paginación básica (Skip/Take)");
        System.Console.WriteLine("-".PadRight(80, '-'));

        int pageSize = 5;
        int pageNumber = 2; // Mostramos la página 2
        int skip = (pageNumber - 1) * pageSize;

        var query = _db.Products
            .OrderBy(p => p.ProductId)
            .Skip(skip)
            .Take(pageSize);

        var products = await query.ToListAsync();

        // Guardar SQL generado
        sb.AppendLine("-- ========================================");
        sb.AppendLine($"-- QUERY 5: Paginación básica");
        sb.AppendLine($"-- Página {pageNumber}, {pageSize} items por página");
        sb.AppendLine("-- Técnica: Skip/Take con OrderBy determinista");
        sb.AppendLine("-- ========================================");
        sb.AppendLine(query.ToQueryString());
        sb.AppendLine();

        // Mostrar resultados en consola
        System.Console.WriteLine($"   Página: {pageNumber}");
        System.Console.WriteLine($"   Items por página: {pageSize}");
        System.Console.WriteLine($"   Resultados obtenidos: {products.Count}");
        System.Console.WriteLine($"   Productos:");
        foreach (var p in products)
        {
            System.Console.WriteLine($"     • [{p.ProductId}] {p.ProductName} - ${p.UnitPrice:F2}");
        }
        System.Console.WriteLine();
    }

    // ========== QUERY 6: Paginación avanzada con metadata ==========
    private async Task RunQuery6PaginationAdvanced(StringBuilder sb)
    {
        System.Console.WriteLine("📋 QUERY 6: Paginación avanzada con PagedResult");
        System.Console.WriteLine("-".PadRight(80, '-'));

        int pageSize = 10;
        int pageNumber = 1;

        var pagedResult = await GetPagedProductsAsync(pageNumber, pageSize);

        // Query para el SQL
        var query = _db.Products
            .OrderBy(p => p.ProductId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDTO(
                p.ProductId,
                p.ProductName,
                p.CategoryId,
                p.UnitPrice,
                p.UnitsInStock
            ));

        // Guardar SQL generado
        sb.AppendLine("-- ========================================");
        sb.AppendLine("-- QUERY 6: Paginación avanzada con DTO");
        sb.AppendLine($"-- Página {pageNumber}/{pagedResult.TotalPages}");
        sb.AppendLine($"-- Total items: {pagedResult.TotalCount}");
        sb.AppendLine("-- Técnica: PagedResult con metadata (TotalPages, HasNext, etc.)");
        sb.AppendLine("-- ========================================");
        sb.AppendLine(query.ToQueryString());
        sb.AppendLine();

        // Mostrar resultados en consola
        System.Console.WriteLine($"   📄 Página: {pagedResult.Page}/{pagedResult.TotalPages}");
        System.Console.WriteLine($"   📊 Items: {pagedResult.Items.Count()}/{pagedResult.TotalCount}");
        System.Console.WriteLine($"   ⬅️  Tiene anterior: {pagedResult.HasPreviousPage}");
        System.Console.WriteLine($"   ➡️  Tiene siguiente: {pagedResult.HasNextPage}");
        System.Console.WriteLine($"   Primeros 5 productos:");
        foreach (var p in pagedResult.Items.Take(5))
        {
            System.Console.WriteLine($"     • [{p.ProductId}] {p.ProductName} - ${p.UnitPrice:F2} ({p.UnitsInStock} en stock)");
        }
        System.Console.WriteLine();
    }

    // ========== QUERY 7: Union de Customers y Suppliers ==========
    private async Task RunQuery7Union(StringBuilder sb)
    {
        System.Console.WriteLine("📋 QUERY 7: Union de Customers y Suppliers");
        System.Console.WriteLine("-".PadRight(80, '-'));

        // Opción 1: Usar Concat en lugar de Union (más eficiente en EF Core)
        var customersQuery = _db.Customers
            .OrderBy(c => c.CustomerId) // 👈 Agregado OrderBy
            .Take(5)
            .Select(c => new ContactInfoDTO(
                c.CustomerId,
                c.CompanyName,
                c.ContactName,
                c.City,
                c.Country,
                "Customer"
            ));

        var suppliersQuery = _db.Suppliers
            .OrderBy(s => s.SupplierId) // 👈 Agregado OrderBy
            .Take(5)
            .Select(s => new ContactInfoDTO(
                s.SupplierId.ToString(),
                s.CompanyName,
                s.ContactName,
                s.City,
                s.Country,
                "Supplier"
            ));

        // Ejecutar queries por separado y combinar en memoria
        var customers = await customersQuery.ToListAsync();
        var suppliers = await suppliersQuery.ToListAsync();

        // Combinar en memoria usando Concat (alternativa a Union)
        var contacts = customers.Concat(suppliers).ToList();

        // Guardar SQL generado
        sb.AppendLine("-- ========================================");
        sb.AppendLine("-- QUERY 7: Union/Concat de Customers y Suppliers");
        sb.AppendLine("-- Técnica: Concat para combinar conjuntos en memoria");
        sb.AppendLine("-- Nota: EF Core ejecuta 2 queries separadas y las combina");
        sb.AppendLine("-- ========================================");
        sb.AppendLine();
        sb.AppendLine("-- Query de Customers:");
        sb.AppendLine(customersQuery.ToQueryString());
        sb.AppendLine();
        sb.AppendLine("-- Query de Suppliers:");
        sb.AppendLine(suppliersQuery.ToQueryString());
        sb.AppendLine();
        sb.AppendLine("-- Combinación: Se ejecuta Concat en memoria después de obtener ambos conjuntos");
        sb.AppendLine("-- Diferencia Union vs Concat: Union elimina duplicados, Concat no");
        sb.AppendLine();

        // Mostrar resultados en consola
        System.Console.WriteLine($"   🔗 Total contactos (Customers: {customers.Count}, Suppliers: {suppliers.Count})");
        System.Console.WriteLine($"   📊 Total combinado: {contacts.Count}");
        System.Console.WriteLine($"   📋 Contactos combinados:");
        foreach (var contact in contacts.OrderBy(c => c.CompanyName))
        {
            System.Console.WriteLine($"     • [{contact.ContactType}] {contact.CompanyName}");
            System.Console.WriteLine($"       Contacto: {contact.ContactName ?? "N/A"}");
            System.Console.WriteLine($"       Ubicación: {contact.City}, {contact.Country}");
        }
        System.Console.WriteLine();
    }

    // ========== QUERY 8: Expresiones compiladas (EF.CompileQuery) ==========
    private async Task RunQuery8CompiledExpressions(StringBuilder sb)
    {
        System.Console.WriteLine("📋 QUERY 8: Expresiones compiladas (EF.CompileQuery)");
        System.Console.WriteLine("-".PadRight(80, '-'));

        // Primera ejecución de query compilada (se cachea el plan)
        var products1 = CompiledProductsPaged(_db, 0, 5).ToList();

        // Segunda ejecución (reutiliza el plan compilado) ⚡
        var products2 = CompiledProductsPaged(_db, 5, 5).ToList();

        // Query compilada para productos caros (ahora Take está dentro)
        var expensiveProducts = CompiledExpensiveProducts(_db, 50m, 5).ToList();

        // Guardar información en SQL
        sb.AppendLine("-- ========================================");
        sb.AppendLine("-- QUERY 8: Expresiones compiladas");
        sb.AppendLine("-- Técnica: EF.CompileQuery() para cachear planes de ejecución");
        sb.AppendLine("-- ========================================");
        sb.AppendLine();
        sb.AppendLine("/*");
        sb.AppendLine("  Expresiones compiladas definidas:");
        sb.AppendLine();
        sb.AppendLine("  1. CompiledProductsPaged:");
        sb.AppendLine("     EF.CompileQuery((ctx, skip, take) => ");
        sb.AppendLine("         ctx.Products.OrderBy(p => p.ProductId).Skip(skip).Take(take))");
        sb.AppendLine();
        sb.AppendLine("  2. CompiledExpensiveProducts:");
        sb.AppendLine("     EF.CompileQuery((ctx, minPrice, take) => ");
        sb.AppendLine("         ctx.Products.Where(p => p.UnitPrice >= minPrice)");
        sb.AppendLine("                     .OrderByDescending(p => p.UnitPrice)");
        sb.AppendLine("                     .Take(take))");
        sb.AppendLine();
        sb.AppendLine("  Ventajas:");
        sb.AppendLine("  - El plan de ejecución se compila una sola vez");
        sb.AppendLine("  - Reutilización eficiente en llamadas subsecuentes");
        sb.AppendLine("  - Mejor rendimiento para queries frecuentes");
        sb.AppendLine("  - Todos los operadores LINQ deben estar dentro de la expresión compilada");
        sb.AppendLine("*/");
        sb.AppendLine();

        // Mostrar resultados en consola
        System.Console.WriteLine($"   ⚡ Query compilada 1 (Skip 0, Take 5):");
        System.Console.WriteLine($"      Resultados: {products1.Count} productos");
        foreach (var p in products1)
        {
            System.Console.WriteLine($"      • [{p.ProductId}] {p.ProductName} - ${p.UnitPrice:F2}");
        }
        System.Console.WriteLine();

        System.Console.WriteLine($"   ⚡ Query compilada 2 (Skip 5, Take 5) [Plan reutilizado]:");
        System.Console.WriteLine($"      Resultados: {products2.Count} productos");
        foreach (var p in products2)
        {
            System.Console.WriteLine($"      • [{p.ProductId}] {p.ProductName} - ${p.UnitPrice:F2}");
        }
        System.Console.WriteLine();

        System.Console.WriteLine($"   💰 Productos caros (> $50) [Query compilada]:");
        System.Console.WriteLine($"      Resultados: {expensiveProducts.Count} productos");
        foreach (var p in expensiveProducts)
        {
            System.Console.WriteLine($"      • [{p.ProductId}] {p.ProductName} - ${p.UnitPrice:F2}");
        }
        System.Console.WriteLine();
    }

    // ========== MÉTODO AUXILIAR: Paginación avanzada con metadata ==========
    private async Task<PagedResult<ProductDTO>> GetPagedProductsAsync(int page, int pageSize)
    {
        // Total de items (query separada)
        var totalCount = await _db.Products.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        // Items de la página actual
        var items = await _db.Products
            .OrderBy(p => p.ProductId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDTO(
                p.ProductId,
                p.ProductName,
                p.CategoryId,
                p.UnitPrice,
                p.UnitsInStock
            ))
            .ToListAsync();

        return new PagedResult<ProductDTO>(
            items,
            page,
            pageSize,
            totalCount,
            totalPages
        );
    }
}