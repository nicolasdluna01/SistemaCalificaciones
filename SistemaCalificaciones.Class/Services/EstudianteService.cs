using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemaCalificaciones.Class.Data;
using SistemaCalificaciones.Class.Models;

namespace SistemaCalificaciones.Class.Services;

public class EstudianteService : IEstudianteService
{
    private readonly SistemaCalificacionesDB _db;
    private readonly UserManager<IdentityUser> _userManager;

    public EstudianteService(SistemaCalificacionesDB db, UserManager<IdentityUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<ResultadoCuenta> CrearAsync(EstudianteModel estudiante)
    {
        if (await _db.Estudiantes.AnyAsync(e => e.Matricula == estudiante.Matricula))
            return Falla("Ya existe un estudiante con esa matrícula.");

        // Cubre también el caso de que un docente ya use ese nombre de usuario
        if (await _userManager.FindByNameAsync(estudiante.Matricula) != null)
            return Falla("Esa matrícula ya está en uso como nombre de usuario.");

        var contrasena = GenerarContrasenaTemporal();

        await using var tx = await _db.Database.BeginTransactionAsync();

        var user = new IdentityUser { UserName = estudiante.Matricula, Email = estudiante.Correo, EmailConfirmed = true };
        var result = await _userManager.CreateAsync(user, contrasena);
        if (!result.Succeeded) return new ResultadoCuenta(result, null);

        result = await _userManager.AddToRoleAsync(user, Roles.Alumno);
        if (!result.Succeeded) return new ResultadoCuenta(result, null);

        estudiante.UserId = user.Id;
        _db.Estudiantes.Add(estudiante);
        await _db.SaveChangesAsync();

        await tx.CommitAsync();
        return new ResultadoCuenta(IdentityResult.Success, contrasena);
    }

    // La matrícula no se modifica: es el usuario con el que inicia sesión
    public async Task<IdentityResult> ActualizarAsync(EstudianteModel datos)
    {
        var estudiante = await _db.Estudiantes.FirstOrDefaultAsync(e => e.Id == datos.Id);
        if (estudiante == null) return Error("El estudiante no existe.");

        await using var tx = await _db.Database.BeginTransactionAsync();

        estudiante.Nombre = datos.Nombre;
        estudiante.ApellidoPaterno = datos.ApellidoPaterno;
        estudiante.ApellidoMaterno = datos.ApellidoMaterno;
        estudiante.Correo = datos.Correo;
        estudiante.Estatus = datos.Estatus;

        if (estudiante.UserId != null)
        {
            var user = await _userManager.FindByIdAsync(estudiante.UserId);
            if (user != null)
            {
                if (user.Email != estudiante.Correo)
                {
                    var result = await _userManager.SetEmailAsync(user, estudiante.Correo);
                    if (!result.Succeeded) return result;
                }
                if (!estudiante.Estatus)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                }
            }
        }

        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        return IdentityResult.Success;
    }

    public async Task<bool?> CambiarEstatusAsync(int id)
    {
        var estudiante = await _db.Estudiantes.FirstOrDefaultAsync(e => e.Id == id);
        if (estudiante == null) return null;

        estudiante.Estatus = !estudiante.Estatus;

        if (!estudiante.Estatus && estudiante.UserId != null)
        {
            var user = await _userManager.FindByIdAsync(estudiante.UserId);
            if (user != null) await _userManager.UpdateSecurityStampAsync(user);
        }

        await _db.SaveChangesAsync();
        return estudiante.Estatus;
    }

    public async Task<ResultadoCuenta> RestablecerContrasenaAsync(int id)
    {
        var estudiante = await _db.Estudiantes.FirstOrDefaultAsync(e => e.Id == id);
        if (estudiante?.UserId == null) return Falla("El estudiante no tiene cuenta de acceso.");

        var user = await _userManager.FindByIdAsync(estudiante.UserId);
        if (user == null) return Falla("No se encontró la cuenta del estudiante.");

        var contrasena = GenerarContrasenaTemporal();
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, contrasena);
        if (!result.Succeeded) return new ResultadoCuenta(result, null);

        // Por si la cuenta estaba bloqueada por intentos fallidos
        await _userManager.SetLockoutEndDateAsync(user, null);
        return new ResultadoCuenta(IdentityResult.Success, contrasena);
    }

    // 10 caracteres con mayúscula, minúscula, número y símbolo (sin caracteres ambiguos como 0/O o 1/l)
    private static string GenerarContrasenaTemporal(int longitud = 10)
    {
        const string mayusculas = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        const string minusculas = "abcdefghijkmnpqrstuvwxyz";
        const string numeros = "23456789";
        const string simbolos = "!@#$%*?";
        const string todos = mayusculas + minusculas + numeros + simbolos;

        var caracteres = new List<char>
        {
            Elegir(mayusculas), Elegir(minusculas), Elegir(numeros), Elegir(simbolos)
        };
        while (caracteres.Count < longitud) caracteres.Add(Elegir(todos));

        return new string(caracteres.OrderBy(_ => RandomNumberGenerator.GetInt32(int.MaxValue)).ToArray());
    }

    private static char Elegir(string origen) => origen[RandomNumberGenerator.GetInt32(origen.Length)];

    private static IdentityResult Error(string mensaje) =>
        IdentityResult.Failed(new IdentityError { Description = mensaje });

    private static ResultadoCuenta Falla(string mensaje) => new(Error(mensaje), null);
}