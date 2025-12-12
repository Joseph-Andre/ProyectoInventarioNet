using LinqAdvancedLab.Data;
using LinqAdvancedLab.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqAdvancedLab.Console;

public class QueryRunner
{
    private readonly NorthwindContext _db;

    public QueryRunner(NorthwindContext db)
    {
        _db = db;
    }

    public async Task RunAsync()
    {
        System.Console.WriteLine("Comprobando conexión a la BD...");
        if (!await _db.Database.CanConnectAsync())
        {
            System.Console.WriteLine("No se puede conectar a la base de datos. Revisa la cadena de conexión.");
            return;
        }

        // Queries deterministas (OrderBy para Take)
        var q1 = _db.Customers.OrderBy(c => c.CustomerId).Take(10);
        var q2 = _db.Orders.Where(o => o.OrderDate != null).OrderByDescending(o => o.OrderDate).Take(10);
        var q3 = _db.Employees.Where(e => e.Title != null).OrderBy(e => e.EmployeeId);
        var q4 = _db.Products.OrderBy(p => p.ProductId).Take(10);

        var sql1 = q1.ToQueryString();
        var sql2 = q2.ToQueryString();
        var sql3 = q3.ToQueryString();
        var sql4 = q4.ToQueryString();

        System.Console.WriteLine("SQL Query 1:\n" + sql1 + "\n");

        // Guardar en la carpeta absoluta docs en la raíz del repo
        var docsDir = @"C:\Users\PC\source\repos\Joseph-Andre\ProyectoInventarioNet\docs";
        Directory.CreateDirectory(docsDir);
        var outPath = Path.Combine(docsDir, "query1.sql");

        var sb = new StringBuilder();
        sb.AppendLine("-- Query 1: Customers (Take 10)");
        sb.AppendLine(sql1);
        sb.AppendLine();
        sb.AppendLine("-- Query 2: Orders (últimos 10 con fecha)");
        sb.AppendLine(sql2);
        sb.AppendLine();
        sb.AppendLine("-- Query 3: Employees (Title != null)");
        sb.AppendLine(sql3);
        sb.AppendLine();
        sb.AppendLine("-- Query 4: Products (Take 10)");
        sb.AppendLine(sql4);

        await File.WriteAllTextAsync(outPath, sb.ToString(), Encoding.UTF8);
        System.Console.WriteLine($"SQL guardado en: {outPath}");
    }
}