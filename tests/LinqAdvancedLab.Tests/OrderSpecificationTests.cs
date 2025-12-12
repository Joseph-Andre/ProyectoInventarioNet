using LinqAdvancedLab.Data.Models;
using LinqAdvancedLab.Data.Repositories;
using LinqAdvancedLab.Data.Specifications.Orders;
using LinqAdvancedLab.Tests.Helpers;
using Xunit;

namespace LinqAdvancedLab.Tests;

/// <summary>
/// Tests para las specifications de Orders
/// </summary>
public class OrderSpecificationTests : IDisposable
{
    private readonly NorthwindContext _context;
    private readonly Repository<Order> _repository;

    public OrderSpecificationTests()
    {
        _context = InMemoryDbContextFactory.Create("TestDb_Orders_" + Guid.NewGuid());
        TestDataSeeder.SeedOrders(_context);
        _repository = new Repository<Order>(_context);
    }

    public void Dispose()
    {
        InMemoryDbContextFactory.Destroy(_context);
    }

    [Fact]
    public async Task OrdersWithDetailsSpecification_ShouldIncludeRelatedEntities()
    {
        // Arrange
        var spec = new OrdersWithDetailsSpecification();

        // Act
        var result = await _repository.GetAsync(spec);

        // Assert
        Assert.NotEmpty(result);
        Assert.All(result, o => Assert.NotNull(o.Customer));
        Assert.All(result, o => Assert.NotNull(o.Employee));
    }

    [Fact]
    public async Task RecentOrdersSpecification_ShouldFilterByDate()
    {
        // Arrange
        var spec = new RecentOrdersSpecification(days: 30);

        // Act
        var result = await _repository.GetAsync(spec);

        // Assert
        Assert.Equal(2, result.Count); // Solo las órdenes de hace 10 y 5 días
    }

    [Fact]
    public async Task RecentOrdersSpecification_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var spec = new RecentOrdersSpecification(days: 30, pageIndex: 0, pageSize: 1);

        // Act
        var result = await _repository.GetAsync(spec);

        // Assert
        Assert.Single(result);
    }
}