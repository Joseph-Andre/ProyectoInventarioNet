using System;
using System.Collections.Generic;
using System.Text;

namespace LinqAdvancedLab.Domain.DTOs;

// DTO para Union de Customers y Suppliers
public record ContactInfoDTO(
    string Id,
    string CompanyName,
    string? ContactName,
    string? City,
    string? Country,
    string ContactType // "Customer" o "Supplier"
);