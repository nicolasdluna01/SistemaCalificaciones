# Sistema de Calificaciones

Aplicación web para que los docentes registren las calificaciones de sus estudiantes por asignatura, grupo y unidad.

## Tecnología

- .NET 8, ASP.NET Core MVC
- Entity Framework Core 8 con SQL Server
- ASP.NET Core Identity

## Estructura

- `SistemaCalificaciones.Class`: modelos, DbContext, repositorios y migraciones
- `SistemaCalificaciones.Web`: aplicación web (MVC)

## Módulos

- [x] Inicio de sesión (Identity, con roles Admin, Docente y Alumno)
- [x] Catálogo de asignaturas (Admin)
- [x] Catálogo de grupos (Admin)
- [ ] Catálogo de docentes
- [ ] Catálogo de estudiantes
- [ ] Asignación de estudiantes y docentes a grupos
- [ ] Captura de calificaciones (Docente)
- [ ] Kardex (Alumno)

## Usuarios de prueba

Al ejecutar la aplicación en el entorno de Desarrollo se crean automáticamente estos usuarios (solo si no existen):

| Rol | Usuario | Contraseña |
|-----|---------|------------|
| Admin | `admin` | `Admin123!` |
| Docente | `docente` | `Docente123!` |

Son únicamente para desarrollo local. Estos usuarios no se crean en otros entornos.

## Configuración local

1. Clona el repositorio.
2. En `SistemaCalificaciones.Web`, copia `appsettings.Development.json.example` como `appsettings.Development.json` y ajusta la cadena de conexión a tu servidor de SQL Server.
3. En la Consola del Administrador de paquetes de Visual Studio, con `SistemaCalificaciones.Web` como proyecto de inicio y `SistemaCalificaciones.Class` como proyecto predeterminado, ejecuta:
```
   Update-Database
```
4. Ejecuta el proyecto `SistemaCalificaciones.Web`.

`appsettings.Development.json` no se sube al repositorio, para que cada colaborador use su propia base de datos.
