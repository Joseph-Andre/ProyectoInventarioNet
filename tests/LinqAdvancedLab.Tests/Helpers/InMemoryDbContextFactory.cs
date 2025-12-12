using System;
using System.Collections.Generic;
using System.Text;

using LinqAdvancedLab.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace LinqAdvancedLab.Tests.Helpers;

/// <summary>
/// Factory para crear instancias de NorthwindContext con base de datos en memoria
/// </summary>
public static class InMemoryDbContextFactory
{
    /// <summary>
    /// Crea un nuevo DbContext con base de datos en memoria
    /// </summary>
    /// <param name="databaseName">Nombre único de la base de datos en memoria</param>
    public static NorthwindContext Create(string databaseName)
    {
        var options = new DbContextOptionsBuilder<NorthwindContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        var context = new NorthwindContext(options);

        // Asegurar que la base de datos esté creada
        context.Database.EnsureCreated();

        return context;
    }

    /// <summary>
    /// Destruye la base de datos en memoria
    /// </summary>
    public static void Destroy(NorthwindContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}