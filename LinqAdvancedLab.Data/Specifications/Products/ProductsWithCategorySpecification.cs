using System;
using System.Collections.Generic;
using System.Text;

using LinqAdvancedLab.Data.Models;

using LinqAdvancedLab.Domain.Specifications; // ← Usa las interfaces de Domain
namespace LinqAdvancedLab.Data.Specifications.Products;


/// <summary>
/// Specification: Obtiene productos con su categoría (eager loading)
/// Útil para evitar N+1 queries
/// </summary>
public class ProductsWithCategorySpecification : BaseSpecification<Product>
{
    /// <summary>
    /// Constructor sin filtros - obtiene todos los productos con categoría
    /// </summary>
    public ProductsWithCategorySpecification()
    {
        AddInclude(p => p.Category!);
        ApplyOrderBy(p => p.ProductName);
    }

    /// <summary>
    /// Constructor con filtro por ID
    /// </summary>
    public ProductsWithCategorySpecification(int productId)
        : base(p => p.ProductId == productId)
    {
        AddInclude(p => p.Category!);
    }
}