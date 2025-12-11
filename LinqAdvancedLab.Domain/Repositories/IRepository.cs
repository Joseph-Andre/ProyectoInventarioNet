using LinqAdvancedLab.Domain.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinqAdvancedLab.Domain.Repositories;

/// <summary>
/// Repositorio genérico con soporte para Specification Pattern
/// Define el contrato para acceso a datos independiente de EF Core
/// </summary>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Obtiene todas las entidades (sin filtros)
    /// </summary>
    Task<IReadOnlyList<T>> GetAllAsync();

    /// <summary>
    /// Obtiene entidades que cumplen con una especificación
    /// </summary>
    /// <param name="spec">Especificación con criterios de consulta</param>
    Task<IReadOnlyList<T>> GetAsync(ISpecification<T> spec);

    /// <summary>
    /// Obtiene una única entidad que cumple con una especificación
    /// </summary>
    /// <param name="spec">Especificación con criterios de consulta</param>
    Task<T?> GetEntityAsync(ISpecification<T> spec);

    /// <summary>
    /// Cuenta entidades que cumplen con una especificación
    /// </summary>
    /// <param name="spec">Especificación con criterios de consulta</param>
    Task<int> CountAsync(ISpecification<T> spec);

    /// <summary>
    /// Verifica si existe alguna entidad que cumpla con la especificación
    /// </summary>
    Task<bool> AnyAsync(ISpecification<T> spec);
}