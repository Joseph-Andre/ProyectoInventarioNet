# ProyectoInventarioNet

## 📂 Entregables Finales

Todo el material solicitado (video demo y diarios combinados en PDF) se encuentra en la siguiente carpeta de Google Drive:

🔗 **Carpeta de entregables: https://drive.google.com/drive/folders/1WdVzjeESo-Iy9VWXxn2yQQ0OoNjlwgBA?usp=sharing

PASOS PARA EJECUTAR LSO QUERY DE 1 AL 4 Y DEL 5 AL 8
ENTRAR EN LA TERMINAL DE :
PS C:\Users\PC\source\repos\Joseph-Andre\ProyectoInventarioNet\LinqAdvancedLab.Console>

SIGUIENTES COMANDOS
--------------
dotnet restore
----------------
dotnet build
----------------
dotnet run
----------------

PASOS PARA EJECUTAR EL TEST
ENTRAR A TERMINAL DE :

PS C:\Users\PC\source\repos\Joseph-Andre\ProyectoInventarioNet\tests\LinqAdvancedLab.Tests>

Paso 2: Ejecutar todos los tests 🧪
------------------
cd tests\LinqAdvancedLab.Tests
------------------------------
dotnet test --verbosity normal EJECUTA TODOS LOS TEST
-------------
Paso 3: Generar reporte de cobertura (≥80%) 📊
-----------------------
dotnet test --collect:"XPlat Code Coverage"
--------------
Para ver el reporte en HTML, instala ReportGenerator:
dotnet tool install -g dotnet-reportgenerator-globaltool
-------------
REPORTE GENERADO 
reportgenerator -reports:"TestResults\**\coverage.cobertura.xml" -targetdir:"TestResults\CoverageReport" -reporttypes:Html
--------------
ABRIR DOCUMENTO
start TestResults\CoverageReport\index.html


