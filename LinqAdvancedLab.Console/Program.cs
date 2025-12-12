using LinqAdvancedLab.Console;
using LinqAdvancedLab.Data.Models;
using LinqAdvancedLab.Data.Repositories;
using LinqAdvancedLab.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<NorthwindContext>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("Northwind")));

        // ✅ Registrar repositorio genérico
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddTransient<QueryRunner>();
        services.AddTransient<AdvancedQueryRunner>();
        services.AddTransient<SpecificationQueryRunner>(); // 👈 Nuevo
    })
    .Build();

using var scope = host.Services.CreateScope();

System.Console.WriteLine("╔════════════════════════════════════════════════════════╗");
System.Console.WriteLine("║     LINQ ADVANCED LAB - Northwind Database            ║");
System.Console.WriteLine("╚════════════════════════════════════════════════════════╝");

// Ejecutar queries básicas 1-4
System.Console.WriteLine("\n🔹 Ejecutando Queries 1-4 (Básicas)...\n");
var basicRunner = scope.ServiceProvider.GetRequiredService<QueryRunner>();
await basicRunner.RunAsync();

// Ejecutar queries avanzadas 5-8
System.Console.WriteLine("\n🔹 Ejecutando Queries 5-8 (Avanzadas)...\n");
var advancedRunner = scope.ServiceProvider.GetRequiredService<AdvancedQueryRunner>();
await advancedRunner.RunAsync();

// ✅ Ejecutar demostración de Specification Pattern
System.Console.WriteLine("\n🔹 Ejecutando Specification Pattern (Refactor)...\n");
var specRunner = scope.ServiceProvider.GetRequiredService<SpecificationQueryRunner>();
await specRunner.RunAsync();

System.Console.WriteLine("\n✅ Todas las queries ejecutadas exitosamente!");
System.Console.WriteLine("\n📁 Archivos generados:");
System.Console.WriteLine("   • docs/query1.sql (Queries 1-4)");
System.Console.WriteLine("   • docs/queries_5_to_8.sql (Queries 5-8)");