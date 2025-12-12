using System;
using System.Collections.Generic;
using System.Text;
using LinqAdvancedLab.Data.Models;

using LinqAdvancedLab.Domain.Specifications;
namespace LinqAdvancedLab.Data.Specifications.Orders;

/// <summary>
/// Specification: Órdenes con detalles, cliente y empleado
/// Demuestra navegación múltiple con Include
/// </summary>
public class OrdersWithDetailsSpecification : BaseSpecification<Order>
{
    public OrdersWithDetailsSpecification()
    {
        AddInclude(o => o.Customer!);
        AddInclude(o => o.Employee!);
        AddInclude(o => o.OrderDetails);
        ApplyOrderByDescending(o => o.OrderDate!);
    }

    public OrdersWithDetailsSpecification(int orderId)
        : base(o => o.OrderId == orderId)
    {
        AddInclude(o => o.Customer!);
        AddInclude(o => o.Employee!);
        AddInclude(o => o.OrderDetails);
    }
}