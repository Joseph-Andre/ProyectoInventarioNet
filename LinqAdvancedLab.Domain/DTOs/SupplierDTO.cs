using System;
using System.Collections.Generic;
using System.Text;

namespace LinqAdvancedLab.Domain.DTOs;
public record SupplierDTO(
    int SupplierId,
    string CompanyName,
    string? ContactName,
    string? Country
);