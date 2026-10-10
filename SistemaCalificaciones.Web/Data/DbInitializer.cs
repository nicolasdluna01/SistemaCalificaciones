using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaCalificaciones.Class.Data;
using SistemaCalificaciones.Class.Models;

namespace SistemaCalificaciones.Web.Data;

public static class DbInitializer
{
    public const string RolDocente = "Docente";

    public static async Task SeedAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<SistemaCalificacionesDB>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        if (!await roleManager.RoleExistsAsync(RolDocente))
        {
            await roleManager.CreateAsync(new IdentityRole(RolDocente));
        }

        const string usuario = "docente";
        const string contrasena = "Docente123!";

        var user = await userManager.FindByNameAsync(usuario);
        if (user == null)
        {
            user = new IdentityUser { UserName = usuario, Email = "docente@prueba.com", EmailConfirmed = true };
            var result = await userManager.CreateAsync(user, contrasena);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    "No se pudo crear el docente de prueba: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            await userManager.AddToRoleAsync(user, RolDocente);
        }

        if (!await db.Docentes.AnyAsync(d => d.Usuario == usuario))
        {
            db.Docentes.Add(new DocenteModel
            {
                NumeroEmpleado = 1001,
                Nombre = "Docente",
                ApellidoPaterno = "De",
                ApellidoMaterno = "Prueba",
                Correo = "docente@prueba.com",
                Usuario = usuario,
                Contrasena = user.PasswordHash!, // copia del hash de Identity
                UserId = user.Id
            });
            await db.SaveChangesAsync();
        }
    }
}