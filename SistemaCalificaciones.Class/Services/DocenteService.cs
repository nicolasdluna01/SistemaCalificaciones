using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaCalificaciones.Class.Data;
using SistemaCalificaciones.Class.Models;

namespace SistemaCalificaciones.Class.Services;

public class DocenteService : IDocenteService
{
    private readonly SistemaCalificacionesDB _db;
    private readonly UserManager<IdentityUser> _userManager;

    public DocenteService(SistemaCalificacionesDB db, UserManager<IdentityUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IdentityResult> CrearAsync(DocenteModel docente, string contrasena)
    {
        if (await _userManager.FindByNameAsync(docente.Usuario) != null)
            return Error("Ese usuario ya está en uso.");

        if (await _db.Docentes.AnyAsync(d => d.NumeroEmpleado == docente.NumeroEmpleado))
            return Error("Ya existe un docente con ese número de empleado.");

        // Si algo falla antes del Commit, al salir se deshace todo (no quedan cuentas huérfanas)
        await using var tx = await _db.Database.BeginTransactionAsync();

        var user = new IdentityUser { UserName = docente.Usuario, Email = docente.Correo, EmailConfirmed = true };
        var result = await _userManager.CreateAsync(user, contrasena);
        if (!result.Succeeded) return result;

        result = await _userManager.AddToRoleAsync(user, Roles.Docente);
        if (!result.Succeeded) return result;

        docente.UserId = user.Id;
        docente.Contrasena = user.PasswordHash!; // copia del hash de Identity
        _db.Docentes.Add(docente);
        await _db.SaveChangesAsync();

        await tx.CommitAsync();
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> ActualizarAsync(DocenteModel datos)
    {
        var docente = await _db.Docentes.FirstOrDefaultAsync(d => d.Id == datos.Id);
        if (docente == null) return Error("El docente no existe.");

        if (await _db.Docentes.AnyAsync(d => d.NumeroEmpleado == datos.NumeroEmpleado && d.Id != datos.Id))
            return Error("Ya existe un docente con ese número de empleado.");

        await using var tx = await _db.Database.BeginTransactionAsync();

        docente.NumeroEmpleado = datos.NumeroEmpleado;
        docente.Nombre = datos.Nombre;
        docente.ApellidoPaterno = datos.ApellidoPaterno;
        docente.ApellidoMaterno = datos.ApellidoMaterno;
        docente.Correo = datos.Correo;
        docente.Estatus = datos.Estatus;

        if (docente.UserId != null)
        {
            var user = await _userManager.FindByIdAsync(docente.UserId);
            if (user != null)
            {
                if (user.Email != docente.Correo)
                {
                    var result = await _userManager.SetEmailAsync(user, docente.Correo);
                    if (!result.Succeeded) return result;
                }
                if (!docente.Estatus)
                {
                    // Invalida la sesión actual del docente desactivado
                    await _userManager.UpdateSecurityStampAsync(user);
                }
            }
        }

        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        return IdentityResult.Success;
    }

    // Devuelve el estatus nuevo, o null si el docente no existe
    public async Task<bool?> CambiarEstatusAsync(int id)
    {
        var docente = await _db.Docentes.FirstOrDefaultAsync(d => d.Id == id);
        if (docente == null) return null;

        docente.Estatus = !docente.Estatus;

        if (!docente.Estatus && docente.UserId != null)
        {
            var user = await _userManager.FindByIdAsync(docente.UserId);
            if (user != null) await _userManager.UpdateSecurityStampAsync(user);
        }

        await _db.SaveChangesAsync();
        return docente.Estatus;
    }

    public async Task<IdentityResult> RestablecerContrasenaAsync(int id, string nuevaContrasena)
    {
        var docente = await _db.Docentes.FirstOrDefaultAsync(d => d.Id == id);
        if (docente?.UserId == null) return Error("El docente no tiene cuenta de acceso.");

        var user = await _userManager.FindByIdAsync(docente.UserId);
        if (user == null) return Error("No se encontró la cuenta del docente.");

        await using var tx = await _db.Database.BeginTransactionAsync();

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, nuevaContrasena);
        if (!result.Succeeded) return result;

        // Por si la cuenta estaba bloqueada por intentos fallidos
        await _userManager.SetLockoutEndDateAsync(user, null);

        docente.Contrasena = user.PasswordHash!;
        await _db.SaveChangesAsync();

        await tx.CommitAsync();
        return IdentityResult.Success;
    }

    private static IdentityResult Error(string mensaje) =>
        IdentityResult.Failed(new IdentityError { Description = mensaje });
}