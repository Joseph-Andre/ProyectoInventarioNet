using System;
using System.Collections.Generic;
using System.Text;

namespace LinqAdvancedLab.Domain.DTOs;
public record CategoryDTO(
    int CategoryId,
    string CategoryName,
    string? Description
);
