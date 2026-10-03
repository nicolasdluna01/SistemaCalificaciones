# Sistema de Calificaciones

Aplicación web para que los docentes registren las calificaciones de sus estudiantes por asignatura, grupo y unidad.

## Tecnología

- .NET 8, ASP.NET Core MVC
- Entity Framework Core 8 con SQL Server
- ASP.NET Core Identity

## Estructura

- `SistemaCalificaciones.Class`: modelos, DbContext, repositorios y migraciones
- `SistemaCalificaciones.Web`: aplicación web (MVC)

## Configuración local

1. Clona el repositorio.
2. En `SistemaCalificaciones.Web`, copia `appsettings.Development.json.example` como `appsettings.Development.json` y ajusta la cadena de conexión a tu servidor de SQL Server.
3. En la Consola del Administrador de paquetes de Visual Studio, con `SistemaCalificaciones.Web` como proyecto de inicio y `SistemaCalificaciones.Class` como proyecto predeterminado, ejecuta:
```
   Update-Database
```
4. Ejecuta el proyecto `SistemaCalificaciones.Web`.

`appsettings.Development.json` no se sube al repositorio, para que cada colaborador use su propia base de datos.