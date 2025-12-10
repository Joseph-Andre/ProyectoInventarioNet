using LinqAdvancedLab.Console;
using LinqAdvancedLab.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<NorthwindContext>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("Northwind")));
        services.AddTransient<QueryRunner>();
    })
    .Build();

using var scope = host.Services.CreateScope();
var runner = scope.ServiceProvider.GetRequiredService<QueryRunner>();
await runner.RunAsync();