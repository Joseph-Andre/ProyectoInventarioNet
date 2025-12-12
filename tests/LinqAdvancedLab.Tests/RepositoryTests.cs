using System;
using System.Collections.Generic;
using System.Text;

using LinqAdvancedLab.Data.Models;
using LinqAdvancedLab.Data.Repositories;
using LinqAdvancedLab.Data.Specifications.Products;
using LinqAdvancedLab.Tests.Helpers;
using Xunit;

namespace LinqAdvancedLab.Tests;

/// <summary>
/// Tests para el repositorio genérico Repository<T>
/// </summary>
public class RepositoryTests : IDisposable
{
    private readonly NorthwindContext _context;
    private readonly Repository<Product> _repository;

    public RepositoryTests()
    {
        // Arrange: Crear contexto en memoria y seedear datos
        _context = InMemoryDbContextFactory.Create("TestDb_Repository_" + Guid.NewGuid());
        TestDataSeeder.SeedProducts(_context);
        _repository = new Repository<Product>(_context);
    }

    public void Dispose()
    {
        InMemoryDbContextFactory.Destroy(_context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(8, result.Count);
    }

    [Fact]
    public async Task GetAsync_WithSpecification_ShouldReturnFilteredProducts()
    {
        // Arrange
        var spec = new ExpensiveProductsSpecification(minPrice: 50m);

        // Act
        var result = await _repository.GetAsync(spec);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count); // Premium Vodka (120), Expensive Wine (75), Salmon (55)
        Assert.All(result, p => Assert.True(p.UnitPrice >= 50m));
    }

    [Fact]
    public async Task GetEntityAsync_WithSpecification_ShouldReturnSingleProduct()
    {
        // Arrange
        var spec = new ProductsWithCategorySpecification(productId: 1);

        // Act
        var result = await _repository.GetEntityAsync(spec);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.ProductId);
        Assert.Equal("Chai", result.ProductName);
        Assert.NotNull(result.Category); // Eager loading
        Assert.Equal("Beverages", result.Category.CategoryName);
    }

    [Fact]
    public async Task CountAsync_WithSpecification_ShouldReturnCorrectCount()
    {
        // Arrange
        var spec = new DiscontinuedProductsSpecification();

        // Act
        var count = await _repository.CountAsync(spec);

        // Assert
        Assert.Equal(1, count); // Solo "Discontinued Item"
    }

    [Fact]
    public async Task AnyAsync_WithSpecification_ShouldReturnTrue_WhenMatchExists()
    {
        // Arrange
        var spec = new ExpensiveProductsSpecification(minPrice: 100m);

        // Act
        var exists = await _repository.AnyAsync(spec);

        // Assert
        Assert.True(exists); // Premium Vodka (120)
    }

    [Fact]
    public async Task AnyAsync_WithSpecification_ShouldReturnFalse_WhenNoMatchExists()
    {
        // Arrange
        var spec = new ExpensiveProductsSpecification(minPrice: 200m);

        // Act
        var exists = await _repository.AnyAsync(spec);

        // Assert
        Assert.False(exists);
    }
}