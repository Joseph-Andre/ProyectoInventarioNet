-- ========================================
-- QUERY 5: Paginación básica
-- Página 2, 5 items por página
-- Técnica: Skip/Take con OrderBy determinista
-- ========================================
DECLARE @p int = 5;

SELECT [p].[ProductID], [p].[CategoryID], [p].[Discontinued], [p].[ProductName], [p].[QuantityPerUnit], [p].[ReorderLevel], [p].[SupplierID], [p].[UnitPrice], [p].[UnitsInStock], [p].[UnitsOnOrder]
FROM [Products] AS [p]
ORDER BY [p].[ProductID]
OFFSET @p ROWS FETCH NEXT @p ROWS ONLY

-- ========================================
-- QUERY 6: Paginación avanzada con DTO
-- Página 1/8
-- Total items: 77
-- Técnica: PagedResult con metadata (TotalPages, HasNext, etc.)
-- ========================================
DECLARE @p int = 0;
DECLARE @p0 int = 10;

SELECT [p].[ProductID], [p].[ProductName], [p].[CategoryID], [p].[UnitPrice], [p].[UnitsInStock]
FROM [Products] AS [p]
ORDER BY [p].[ProductID]
OFFSET @p ROWS FETCH NEXT @p0 ROWS ONLY

-- ========================================
-- QUERY 7: Union/Concat de Customers y Suppliers
-- Técnica: Concat para combinar conjuntos en memoria
-- Nota: EF Core ejecuta 2 queries separadas y las combina
-- ========================================

-- Query de Customers:
DECLARE @p int = 5;

SELECT TOP(@p) [c].[CustomerID], [c].[CompanyName], [c].[ContactName], [c].[City], [c].[Country]
FROM [Customers] AS [c]
ORDER BY [c].[CustomerID]

-- Query de Suppliers:
DECLARE @p int = 5;

SELECT TOP(@p) CONVERT(varchar(11), [s].[SupplierID]), [s].[CompanyName], [s].[ContactName], [s].[City], [s].[Country]
FROM [Suppliers] AS [s]
ORDER BY [s].[SupplierID]

-- Combinación: Se ejecuta Concat en memoria después de obtener ambos conjuntos
-- Diferencia Union vs Concat: Union elimina duplicados, Concat no

-- ========================================
-- QUERY 8: Expresiones compiladas
-- Técnica: EF.CompileQuery() para cachear planes de ejecución
-- ========================================

/*
  Expresiones compiladas definidas:

  1. CompiledProductsPaged:
     EF.CompileQuery((ctx, skip, take) => 
         ctx.Products.OrderBy(p => p.ProductId).Skip(skip).Take(take))

  2. CompiledExpensiveProducts:
     EF.CompileQuery((ctx, minPrice, take) => 
         ctx.Products.Where(p => p.UnitPrice >= minPrice)
                     .OrderByDescending(p => p.UnitPrice)
                     .Take(take))

  Ventajas:
  - El plan de ejecución se compila una sola vez
  - Reutilización eficiente en llamadas subsecuentes
  - Mejor rendimiento para queries frecuentes
  - Todos los operadores LINQ deben estar dentro de la expresión compilada
*/

