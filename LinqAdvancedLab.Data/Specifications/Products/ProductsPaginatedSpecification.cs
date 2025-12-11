using System;
using System.Collections.Generic;
using System.Text;

using LinqAdvancedLab.Data.Models;
using LinqAdvancedLab.Domain.Specifications; // ← Usa las interfaces de Domain
namespace LinqAdvancedLab.Data.Specifications.Products;

/// <summary>
/// Specification: Productos paginados ordenados por ID
/// Útil para listados generales con paginación
/// </summary>
public class ProductsPaginatedSpecification : BaseSpecification<Product>
{
    public ProductsPaginatedSpecification(int pageIndex, int pageSize)
    {
        ApplyOrderBy(p => p.ProductId);
        ApplyPaging(pageIndex * pageSize, pageSize);
    }
}