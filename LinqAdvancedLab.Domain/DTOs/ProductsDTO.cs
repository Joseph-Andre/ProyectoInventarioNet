using System;
using System.Collections.Generic;
using System.Text;

namespace LinqAdvancedLab.Domain.DTOs;
public record ProductDTO(
    int ProductId,
    string ProductName,
    int? CategoryId,
    decimal? UnitPrice,
    short? UnitsInStock
);