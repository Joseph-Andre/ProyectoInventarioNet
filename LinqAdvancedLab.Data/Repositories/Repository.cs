using LinqAdvancedLab.Data.Models;
using LinqAdvancedLab.Domain.Repositories;
using LinqAdvancedLab.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinqAdvancedLab.Data.Repositories;

/// <summary>
/// Implementación genérica del repositorio usando EF Core
/// Traduce las especificaciones a consultas IQueryable
/// </summary>
public class Repository<T> : IRepository<T> where T : class
{
    private readonly NorthwindContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(NorthwindContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<T?> GetEntityAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).CountAsync();
    }

    public async Task<bool> AnyAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).AnyAsync();
    }

    /// <summary>
    /// Aplica la especificación al DbSet usando el evaluador
    /// </summary>
    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), spec);
    }
}