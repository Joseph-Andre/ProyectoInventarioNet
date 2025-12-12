using System;
using System.Collections.Generic;
using System.Text;

using LinqAdvancedLab.Data.Models;
using LinqAdvancedLab.Domain.Specifications; // ← Usa las interfaces de Domain
namespace LinqAdvancedLab.Data.Specifications.Products;


/// <summary>
/// Specification: Productos con precio >= minPrice
/// Ordenados descendentemente por precio
/// </summary>
public class ExpensiveProductsSpecification : BaseSpecification<Product>
{
    /// <summary>
    /// Constructor básico - productos caros sin paginación
    /// </summary>
    public ExpensiveProductsSpecification(decimal minPrice)
        : base(p => p.UnitPrice >= minPrice)
    {
        ApplyOrderByDescending(p => p.UnitPrice!);
    }

    /// <summary>
    /// Constructor con paginación
    /// </summary>
    public ExpensiveProductsSpecification(decimal minPrice, int pageIndex, int pageSize)
        : base(p => p.UnitPrice >= minPrice)
    {
        ApplyOrderByDescending(p => p.UnitPrice!);
        ApplyPaging(pageIndex * pageSize, pageSize);
    }
}