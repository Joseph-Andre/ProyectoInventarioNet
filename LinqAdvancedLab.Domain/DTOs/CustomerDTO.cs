using System;
using System.Collections.Generic;
using System.Text;

namespace LinqAdvancedLab.Domain.DTOs;

public record CustomerDTO(
    string CustomerId,
    string CompanyName,
    string? ContactName,
    string? City,
    string? Country
);
