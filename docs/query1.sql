-- Query 1: Customers (Take 10)
DECLARE @p int = 10;

SELECT TOP(@p) [c].[CustomerID], [c].[Address], [c].[City], [c].[CompanyName], [c].[ContactName], [c].[ContactTitle], [c].[Country], [c].[Fax], [c].[Phone], [c].[PostalCode], [c].[Region]
FROM [Customers] AS [c]
ORDER BY [c].[CustomerID]

-- Query 2: Orders (últimos 10 con fecha)
DECLARE @p int = 10;

SELECT TOP(@p) [o].[OrderID], [o].[CustomerID], [o].[EmployeeID], [o].[Freight], [o].[OrderDate], [o].[RequiredDate], [o].[ShipAddress], [o].[ShipCity], [o].[ShipCountry], [o].[ShipName], [o].[ShipPostalCode], [o].[ShipRegion], [o].[ShipVia], [o].[ShippedDate]
FROM [Orders] AS [o]
WHERE [o].[OrderDate] IS NOT NULL
ORDER BY [o].[OrderDate] DESC

-- Query 3: Employees (Title != null)
SELECT [e].[EmployeeID], [e].[Address], [e].[BirthDate], [e].[City], [e].[Country], [e].[Extension], [e].[FirstName], [e].[HireDate], [e].[HomePhone], [e].[LastName], [e].[Notes], [e].[Photo], [e].[PhotoPath], [e].[PostalCode], [e].[Region], [e].[ReportsTo], [e].[Title], [e].[TitleOfCourtesy]
FROM [Employees] AS [e]
WHERE [e].[Title] IS NOT NULL
ORDER BY [e].[EmployeeID]

-- Query 4: Products (Take 10)
DECLARE @p int = 10;

SELECT TOP(@p) [p].[ProductID], [p].[CategoryID], [p].[Discontinued], [p].[ProductName], [p].[QuantityPerUnit], [p].[ReorderLevel], [p].[SupplierID], [p].[UnitPrice], [p].[UnitsInStock], [p].[UnitsOnOrder]
FROM [Products] AS [p]
ORDER BY [p].[ProductID]
