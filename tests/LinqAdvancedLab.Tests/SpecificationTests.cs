using LinqAdvancedLab.Data.Models;
using LinqAdvancedLab.Data.Repositories;
using LinqAdvancedLab.Data.Specifications.Products;
using LinqAdvancedLab.Tests.Helpers;
using Xunit;

namespace LinqAdvancedLab.Tests;

/// <summary>
/// Tests para las specifications concretas
/// </summary>
public class SpecificationTests : IDisposable
{
    private readonly NorthwindContext _context;
    private readonly Repository<Product> _repository;

    public SpecificationTests()
    {
        _context = InMemoryDbContextFactory.Create("TestDb_Spec_" + Guid.NewGuid());
        TestDataSeeder.SeedProducts(_context);
        _repository = new Repository<Product>(_context);
    }

    public void Dispose()
    {
        InMemoryDbContextFactory.Destroy(_context);
    }

    [Fact]
    public async Task ProductsWithCategorySpecification_ShouldIncludeCategory()
    {
        // Arrange
        var spec = new ProductsWithCategorySpecification();

        // Act
        var result = await _repository.GetAsync(spec);

        // Assert
        Assert.NotEmpty(result);
        Assert.All(result, p => Assert.NotNull(p.Category));
    }

    [Theory]
    [InlineData(50, 3)] // 3 productos >= $50
    [InlineData(20, 5)] // 5 productos >= $20
    [InlineData(100, 1)] // 1 producto >= $100
    public async Task ExpensiveProductsSpecification_ShouldFilterByPrice(decimal minPrice, int expectedCount)
    {
        // Arrange
        var spec = new ExpensiveProductsSpecification(minPrice);

        // Act
        var result = await _repository.GetAsync(spec);

        // Assert
        Assert.Equal(expectedCount, result.Count);
        Assert.All(result, p => Assert.True(p.UnitPrice >= minPrice));
    }

    [Fact]
    public async Task ProductsPaginatedSpecification_ShouldReturnCorrectPage()
    {
        // Arrange
        int pageIndex = 1; // Página 2
        int pageSize = 3;
        var spec = new ProductsPaginatedSpecification(pageIndex, pageSize);

        // Act
        var result = await _repository.GetAsync(spec);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(4, result[0].ProductId); // Debe empezar en el producto 4
    }

    [Fact]
    public async Task DiscontinuedProductsSpecification_ShouldReturnOnlyDiscontinued()
    {
        // Arrange
        var spec = new DiscontinuedProductsSpecification();

        // Act
        var result = await _repository.GetAsync(spec);

        // Assert
        Assert.Single(result);
        Assert.True(result[0].Discontinued);
        Assert.Equal("Discontinued Item", result[0].ProductName);
    }

    [Fact]
    public async Task ProductsByCategorySpecification_ShouldFilterByCategory()
    {
        // Arrange
        var spec = new ProductsByCategorySpecification(categoryId: 1); // Beverages

        // Act
        var result = await _repository.GetAsync(spec);

        // Assert
        Assert.Equal(4, result.Count); // Chai, Chang, Expensive Wine, Premium Vodka
        Assert.All(result, p => Assert.Equal(1, p.CategoryId));
        Assert.All(result, p => Assert.NotNull(p.Category));
    }
}