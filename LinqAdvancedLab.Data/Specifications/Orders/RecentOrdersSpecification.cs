using System;
using System.Collections.Generic;
using System.Text;
using LinqAdvancedLab.Data.Models;
using System;
using LinqAdvancedLab.Domain.Specifications;
namespace LinqAdvancedLab.Data.Specifications.Orders;

/// <summary>
/// Specification: Órdenes recientes (últimos N días)
/// </summary>
public class RecentOrdersSpecification : BaseSpecification<Order>
{
    public RecentOrdersSpecification(int days)
        : base(o => o.OrderDate >= DateTime.Now.AddDays(-days))
    {
        AddInclude(o => o.Customer!);
        ApplyOrderByDescending(o => o.OrderDate!);
    }

    public RecentOrdersSpecification(int days, int pageIndex, int pageSize)
        : base(o => o.OrderDate >= DateTime.Now.AddDays(-days))
    {
        AddInclude(o => o.Customer!);
        ApplyOrderByDescending(o => o.OrderDate!);
        ApplyPaging(pageIndex * pageSize, pageSize);
    }
}