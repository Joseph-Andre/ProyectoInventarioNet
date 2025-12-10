using System;
using System.Collections.Generic;
using System.Text;

namespace LinqAdvancedLab.Domain.DTOs;
public record EmployeeDTO(
    int EmployeeId,
    string FirstName,
    string LastName,
    string? Title
);
