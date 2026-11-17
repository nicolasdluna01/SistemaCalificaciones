using Microsoft.AspNetCore.Identity;
using SistemaCalificaciones.Class.Models;

namespace SistemaCalificaciones.Class.Services;

// ContrasenaTemporal solo trae valor cuando la operación generó una contraseña
public record ResultadoCuenta(IdentityResult Resultado, string? ContrasenaTemporal);

public interface IEstudianteService
{
    Task<ResultadoCuenta> CrearAsync(EstudianteModel estudiante);
    Task<IdentityResult> ActualizarAsync(EstudianteModel datos);
    Task<bool?> CambiarEstatusAsync(int id);
    Task<ResultadoCuenta> RestablecerContrasenaAsync(int id);
}