Markdown

# Diario de Aprendizaje – <Joseph> – Semana <10/12/2025>
## 1. Objetivos del día
- [X] Realizar modelado inicial (Scaffold + DTOs).
- [X] Implementar consultas básicas (Queries 1–4).
- [X]  Generar SQL con ToQueryString() para documentación.
- [X]  Realizar PRs hacia la rama develop.
## 2. Lo que logré
-Realicé Scaffold del Northwind, generé los DTOs y confirmé cambios con el commit “feat: scaffold + DTOs”.
-Completé las Queries 1–4 del laboratorio y generé los archivos SQL en docs/query1.sql.
-Realicé Push + Pull Request a develop y el PR fue aprobado por otro equipo (code review).
-Moví la tarjeta “Modelado” de Doing → Done en el tablero del proyecto.
## 3. Dificultades
-El proceso de organizar los archivos generados por el Scaffold tomó más tiempo de lo esperado.
-Tuve inconvenientes al crear algunos archivos y ubicarlos dentro de la estructura correcta del proyecto.
- También surgieron problemas al ejecutar los comandos de migración, ya que varios requerían ajustes en el contexto y rutas antes de funcionar correctamente.
- Ajustar los DTOs requirió revisar nombres y mapeos para evitar inconsistencias.
## 4. Próximo paso
-Avanzar con las Queries 5–8 (paginación, unión y consultas compiladas).
## 5. Tiempo invertido
-Modelado: 2 h
-Consultas básicas (Queries 1–4): 2 h
-Total: 4 h


# Diario de Aprendizaje – <Joseph> – Semana <11/12/2025> (continuación)
## 1. Objetivos del día (Punto 2)
- [X] Crear rama feature/repository-specification.
- [X] Diseñar interfaces `ISpecification<T>` e `IRepository<T>` en Domain.
- [X] Implementar `BaseSpecification<T>` con métodos fluent.
- [X] Implementar `Repository<T>` genérico en Data con EF Core.
- [X] Crear `SpecificationEvaluator<T>` para traducir specs a IQueryable.
- [X] Implementar 7 specifications concretas (5 Products, 2 Orders).
- [X] Crear `SpecificationQueryRunner.cs` para demostrar el patrón.
- [X] Registrar repositorio genérico en DI (Program.cs).
- [X] Ejecutar y validar funcionamiento completo.

## 2. Lo que logré (Punto 2)
- **Arquitectura implementada**:
  - **Domain** (interfaces puras, sin dependencias):
    - `ISpecification<T>`: Contrato con Criteria, Includes, OrderBy, Paging
    - `BaseSpecification<T>`: Implementación base con métodos fluent
    - `IRepository<T>`: Contrato del repositorio genérico
  - **Data** (implementaciones + specifications concretas):
    - `Repository<T>`: Implementación genérica usando `NorthwindContext`
    - `SpecificationEvaluator<T>`: Traduce specs a IQueryable (WHERE → ORDER BY → INCLUDE → PAGING)
    - **Specifications Products** (5):
      - `ProductsWithCategorySpecification`: Eager loading con Include
      - `ExpensiveProductsSpecification`: Filtro por precio + paginación
      - `ProductsPaginatedSpecification`: Paginación genérica
      - `DiscontinuedProductsSpecification`: Productos descontinuados
      - `ProductsByCategorySpecification`: Filtro por categoría con Include
    - **Specifications Orders** (2):
      - `OrdersWithDetailsSpecification`: Múltiples Includes (Customer, Employee, OrderDetails)
      - `RecentOrdersSpecification`: Filtro por fecha + paginación
  - **Console**:
    - `SpecificationQueryRunner.cs`: Demostración de 7 casos de uso con salida en consola
- **Registro en DI**: `services.AddScoped(typeof(IRepository<>), typeof(Repository<>))`
- **Ejecución exitosa**: 
  - 77 productos con categoría (LEFT JOIN)
  - 7 productos caros paginados (WHERE + ORDER BY DESC + OFFSET-FETCH)
  - 10 productos paginados (página 2)
  - 8 productos descontinuados
  - 12 productos en categoría Beverages
  - 830 órdenes con 3 includes (Customer, Employee, OrderDetails)
  - 0 órdenes recientes (data antigua de Northwind)

## 3. Dificultades (Punto 2)
- **Ciclo de dependencias circular**: 
  - Inicialmente puse las specifications en `Domain`, que referenció a `Data` para usar las entidades.
  - Pero `Data` ya referenciaba a `Domain` para las interfaces.
  - **Solución**: Mover las specifications concretas de `Domain.Specifications` a `Data.Specifications`. Domain solo contiene las interfaces y `BaseSpecification<T>`.
- **Error en DI**: 
  - `Repository<T>` inicialmente pedía `DbContext` genérico en el constructor.
  - DI no podía resolver porque solo `NorthwindContext` estaba registrado.
  - **Solución**: Cambiar el constructor para recibir `NorthwindContext` directamente.
- **Arquitectura vs pragmatismo**:
  - En Clean Architecture ideal, las entidades deberían estar en `Domain`, no en `Data`.
  - Pero como ya teníamos el scaffold de EF Core en `Data.Models`, mover todo sería mucho trabajo.
  - **Decisión**: Mantener entidades en Data y documentar esta limitación en el diario.

## 4. Aprendizajes clave (Punto 2)
- **Patrón Specification**: 
  - Encapsula criterios de consulta reutilizables.
  - Separa lógica de negocio (Domain) de infraestructura (Data).
  - Permite componer queries complejas con criterios, ordenamiento, paginación e includes.
- **Repositorio genérico**:
  - `IRepository<T>` define métodos: `GetAsync(spec)`, `GetEntityAsync(spec)`, `CountAsync(spec)`, `AnyAsync(spec)`.
  - La implementación usa `SpecificationEvaluator<T>` para construir el IQueryable.
- **SpecificationEvaluator**:
  - Respeta el orden crítico de operadores: **WHERE → ORDER BY → INCLUDE → PAGING**.
  - `ApplyPaging` debe ir al final después de OrderBy, o SQL falla.
- **SQL dinámico vs estático**:
  - Las specifications generan SQL en **runtime** según parámetros.
  - No tiene sentido documentar en `.sql` estático (a diferencia de Queries 1-8).
  - La demostración en **consola interactiva** es la mejor forma de validar el patrón.
- **Ventajas demostradas**:
  - Reutilización (misma spec con diferentes parámetros)
  - Testabilidad (specifications son POCOs sin EF Core)
  - Composición (combinar criterios fácilmente)
  - Mantenibilidad (lógica de consulta encapsulada)
  - SQL eficiente (EF Core traduce correctamente)

## 5. Próximo paso
- Implementar **Punto 3**: Tests con **xUnit + EF In-Memory** (≥80% cobertura).
- Testear `Repository<T>` y specifications principales.
- Generar reporte de cobertura de código.

## 6. Tiempo invertido (Punto 2)
- Diseño de interfaces (ISpecification, IRepository): 30 min
- Implementación de BaseSpecification y Repository: 45 min
- Implementación de SpecificationEvaluator: 30 min
- Creación de 7 specifications concretas: 1 h
- SpecificationQueryRunner (demostración): 40 min
- Debugging ciclo de dependencias: 25 min
- Debugging error de DI (DbContext): 15 min
- Pruebas y validación completa: 20 min
- Documentación y reflexión: 25 min
- **Total: 4 h 30 min**