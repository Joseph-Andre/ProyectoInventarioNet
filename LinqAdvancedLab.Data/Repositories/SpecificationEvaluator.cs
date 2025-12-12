using LinqAdvancedLab.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LinqAdvancedLab.Data.Repositories;

/// <summary>
/// Evalúa y aplica especificaciones a un IQueryable de EF Core
/// Construye la query final respetando el orden: WHERE → ORDER BY → INCLUDE → PAGING
/// </summary>
public class SpecificationEvaluator<T> where T : class
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
    {
        var query = inputQuery;

        // 1. Aplicar filtro WHERE (Criteria)
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        // 2. Aplicar ordenamiento (OrderBy o OrderByDescending)
        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }
        else if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        // 3. Aplicar includes por expresión (eager loading)
        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        // 4. Aplicar includes por string (navegación anidada)
        query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

        // 5. Aplicar paginación (DEBE SER LO ÚLTIMO después de OrderBy)
        if (spec.IsPagingEnabled)
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }

        return query;
    }
}