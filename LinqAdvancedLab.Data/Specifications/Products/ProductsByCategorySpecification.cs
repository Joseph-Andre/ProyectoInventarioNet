using System;
using System.Collections.Generic;
using System.Text;

using LinqAdvancedLab.Data.Models;
using LinqAdvancedLab.Domain.Specifications; // ← Usa las interfaces de Domain
namespace LinqAdvancedLab.Data.Specifications.Products;


/// <summary>
/// Specification: Productos filtrados por categoría
/// </summary>
public class ProductsByCategorySpecification : BaseSpecification<Product>
{
    public ProductsByCategorySpecification(int categoryId)
        : base(p => p.CategoryId == categoryId)
    {
        AddInclude(p => p.Category!);
        ApplyOrderBy(p => p.ProductName);
    }
}