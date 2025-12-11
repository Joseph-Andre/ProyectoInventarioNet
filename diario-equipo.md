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

# Diario de Aprendizaje – <Joseph> – Semana <11/12/2025>
## 1. Objetivos del día
- [X] Crear la rama feature/queries5-8.
- [X] Implementar Query 5 (paginación básica con Skip/Take).
- [X] Implementar Query 6 (paginación avanzada con PagedResult).
- [X] Implementar Query 7 (Union/Concat de Customers y Suppliers).
- [X] Implementar Query 8 (expresiones compiladas con EF.CompileQuery).
- [X] Generar SQL documentado en docs/queries_5_to_8.sql.
- [X] Crear la Pull Request hacia develop.

## 2. Lo que logré
- **Creación de rama**: Creé la rama `feature/queries5-8` desde `develop` para trabajar las consultas avanzadas.
- **Query 5 (Paginación básica)**: Implementé paginación con `Skip()` y `Take()`, el SQL genera `OFFSET-FETCH` correctamente.
- **Query 6 (Paginación avanzada)**: Creé el DTO `PagedResult<T>` con metadata completa (TotalPages, HasPreviousPage, HasNextPage), útil para interfaces de usuario con paginación.
- **Query 7 (Union/Concat)**: Combiné Customers y Suppliers en un DTO unificado `ContactInfoDTO` usando `Concat()` en memoria (aprendí que EF Core no puede traducir `Union` después de proyecciones complejas).
- **Query 8 (Expresiones compiladas)**: Implementé `EF.CompileQuery()` para cachear planes de ejecución. Tuve que corregir un error donde `.Take()` debe estar dentro de la expresión compilada, no después.
- **Archivos creados**:
  - `AdvancedQueryRunner.cs` (maneja queries 5-8 separadas de las básicas)
  - `PagedResult.cs`, `ContactInfoDTO.cs`, `OrderDTO.cs`, `EmployeeDTO.cs`, `CustomerDTO.cs`
  - `docs/queries_5_to_8.sql` con SQL generado y documentado
- **Commit realizado**: `feat: queries 5-8 (paginación, union, compiled queries)`
- **Push y PR**: Abrí Pull Request hacia `develop` y actualicé el tablero del proyecto.

## 3. Dificultades
- **Error en Query 7 (Union)**: EF Core no puede traducir `.Union()` después de proyectar a DTOs complejos. **Solución**: Ejecutar queries por separado con `.ToListAsync()` y usar `.Concat()` en memoria.
- **Error en Query 8 (Expresiones compiladas)**: Intenté aplicar `.Take()` después de ejecutar la query compilada, pero el tipo de retorno (`IEnumerable<Product>`) no era compatible con `.OrderByDescending()`. **Solución**: Mover `.Take()` dentro de la expresión compilada y agregar el parámetro `int take` al `Func<>`.
- **Referencia de proyecto faltante**: El proyecto `LinqAdvancedLab.Console` no tenía referencia a `LinqAdvancedLab.Domain`, causando errores de compilación. **Solución**: Agregué `<ProjectReference Include="..\LinqAdvancedLab.Domain\LinqAdvancedLab.Domain.csproj" />` al `.csproj`.
- **Archivo queries_5_to_8.sql no se generaba**: El programa se detenía por el error en Query 8 antes de llegar al código de guardado. Después de corregir Query 8, el archivo se generó correctamente.

## 4. Aprendizajes clave
- **Paginación con metadata**: `PagedResult<T>` es un patrón profesional para APIs paginadas (incluye TotalCount, TotalPages, HasNext/Previous).
- **Union vs Concat en EF Core**: `Union` elimina duplicados pero no funciona después de proyecciones complejas; `Concat` es más flexible.
- **Expresiones compiladas**: `EF.CompileQuery()` compila el árbol de expresión una sola vez y lo reutiliza, mejorando rendimiento en queries repetitivas. **Importante**: Todos los operadores LINQ deben estar dentro de la expresión compilada.
- **ToQueryString()**: Muy útil para debugging y documentación de SQL generado por EF Core.

## 5. Próximo paso
- Implementar **Punto 2**: Refactor con **repositorio genérico** + **Specification pattern**.
- Crear interfaces `IRepository<T>` y `ISpecification<T>`.
- Refactorizar queries existentes para usar el nuevo patrón.

## 6. Tiempo invertido
- Query 5 (paginación básica): 30 min
- Query 6 (paginación avanzada + PagedResult): 45 min
- Query 7 (Union/Concat): 40 min (incluye debugging del error de Union)
- Query 8 (expresiones compiladas): 50 min (incluye corrección del error de Take)
- Creación de DTOs adicionales: 20 min
- Debugging y correcciones: 30 min
- Documentación y commit: 15 min
- **Total: 3 h 30 min**
- 