using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LinqAdvancedLab.Domain.Specifications;

/// <summary>
/// Patrón Specification: Encapsula criterios de consulta reutilizables
/// Permite componer lógica de negocio separada de la infraestructura de datos
/// </summary>
/// <typeparam name="T">Tipo de entidad</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Expresión de filtro (WHERE clause)
    /// </summary>
    Expression<Func<T, bool>>? Criteria { get; }

    /// <summary>
    /// Lista de expresiones para eager loading (Include)
    /// </summary>
    List<Expression<Func<T, object>>> Includes { get; }

    /// <summary>
    /// Lista de includes mediante string (para navegación anidada)
    /// </summary>
    List<string> IncludeStrings { get; }

    /// <summary>
    /// Expresión de ordenamiento ascendente
    /// </summary>
    Expression<Func<T, object>>? OrderBy { get; }

    /// <summary>
    /// Expresión de ordenamiento descendente
    /// </summary>
    Expression<Func<T, object>>? OrderByDescending { get; }

    /// <summary>
    /// Cantidad de registros a tomar (para paginación)
    /// </summary>
    int Take { get; }

    /// <summary>
    /// Cantidad de registros a saltar (para paginación)
    /// </summary>
    int Skip { get; }

    /// <summary>
    /// Indica si la paginación está habilitada
    /// </summary>
    bool IsPagingEnabled { get; }
}