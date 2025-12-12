using System;
using System.Collections.Generic;
using System.Text;

namespace LinqAdvancedLab.Domain.DTOs;

public record OrderDTO(
    int OrderId,
    string? CustomerId,
    DateTime? OrderDate,
    decimal? Freight,
    string? ShipCity
);
