using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaCalificaciones.Class.Data;
using SistemaCalificaciones.Class.Models;

namespace SistemaCalificaciones.Web.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var db = services.GetRequiredService<SistemaCalificacionesDB>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        foreach (var rol in new[] { Roles.Admin, Roles.Docente, Roles.Alumno })
        {
            if (!await roleManager.RoleExistsAsync(rol))
            {
                await roleManager.CreateAsync(new IdentityRole(rol));
            }
        }

        // El admin solo es una cuenta de Identity; no tiene registro en Docentes
        await CrearUsuarioAsync(userManager, "admin", "admin@prueba.com", "Admin123!", Roles.Admin);

        var userDocente = await CrearUsuarioAsync(userManager, "docente", "docente@prueba.com", "Docente123!", Roles.Docente);

        if (!await db.Docentes.AnyAsync(d => d.Usuario == "docente"))
        {
            db.Docentes.Add(new DocenteModel
            {
                NumeroEmpleado = 1001,
                Nombre = "Docente",
                ApellidoPaterno = "De",
                ApellidoMaterno = "Prueba",
                Correo = "docente@prueba.com",
                Usuario = "docente",
                Contrasena = userDocente.PasswordHash!, // copia del hash de Identity
                UserId = userDocente.Id
            });
            await db.SaveChangesAsync();
        }
    }

    private static async Task<IdentityUser> CrearUsuarioAsync(
        UserManager<IdentityUser> userManager, string usuario, string correo, string contrasena, string rol)
    {
        var user = await userManager.FindByNameAsync(usuario);
        if (user != null) return user;

        user = new IdentityUser { UserName = usuario, Email = correo, EmailConfirmed = true };
        var result = await userManager.CreateAsync(user, contrasena);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                $"No se pudo crear el usuario '{usuario}': " + string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        await userManager.AddToRoleAsync(user, rol);
        return user;
    }
}