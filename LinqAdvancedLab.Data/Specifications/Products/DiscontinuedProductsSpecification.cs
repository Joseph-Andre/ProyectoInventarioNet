using System;
using System.Collections.Generic;
using System.Text;

using LinqAdvancedLab.Data.Models;
using LinqAdvancedLab.Domain.Specifications; // ← Usa las interfaces de Domain
namespace LinqAdvancedLab.Data.Specifications.Products;


/// <summary>
/// Specification: Productos descontinuados
/// </summary>
public class DiscontinuedProductsSpecification : BaseSpecification<Product>
{
    public DiscontinuedProductsSpecification()
        : base(p => p.Discontinued)
    {
        ApplyOrderBy(p => p.ProductName);
    }
}