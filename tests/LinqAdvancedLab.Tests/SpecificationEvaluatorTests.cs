using LinqAdvancedLab.Data.Models;
using LinqAdvancedLab.Data.Repositories;
using LinqAdvancedLab.Domain.Specifications;
using LinqAdvancedLab.Tests.Helpers;
using Xunit;

namespace LinqAdvancedLab.Tests;

/// <summary>
/// Tests para SpecificationEvaluator
/// </summary>
public class SpecificationEvaluatorTests : IDisposable
{
    private readonly NorthwindContext _context;

    public SpecificationEvaluatorTests()
    {
        _context = InMemoryDbContextFactory.Create("TestDb_Evaluator_" + Guid.NewGuid());
        TestDataSeeder.SeedProducts(_context);
    }

    public void Dispose()
    {
        InMemoryDbContextFactory.Destroy(_context);
    }

    [Fact]
    public void GetQuery_WithNoCriteria_ShouldReturnAll()
    {
        // Arrange
        var spec = new TestSpecification(); // Sin filtros
        var query = _context.Products.AsQueryable();

        // Act
        var result = SpecificationEvaluator<Product>.GetQuery(query, spec);

        // Assert
        Assert.Equal(8, result.Count());
    }

    [Fact]
    public void GetQuery_WithOrderBy_ShouldApplyOrdering()
    {
        // Arrange
        var spec = new TestSpecificationWithOrderBy();
        var query = _context.Products.AsQueryable();

        // Act
        var result = SpecificationEvaluator<Product>.GetQuery(query, spec).ToList();

        // Assert
        Assert.Equal("Aniseed Syrup", result.First().ProductName); // Primero alfabéticamente
    }
}

// Helper specs para testing
public class TestSpecification : BaseSpecification<Product>
{
}

public class TestSpecificationWithOrderBy : BaseSpecification<Product>
{
    public TestSpecificationWithOrderBy()
    {
        ApplyOrderBy(p => p.ProductName);
    }
}