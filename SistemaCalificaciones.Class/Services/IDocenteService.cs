using Microsoft.AspNetCore.Identity;
using SistemaCalificaciones.Class.Models;

namespace SistemaCalificaciones.Class.Services;

public interface IDocenteService
{
    Task<IdentityResult> CrearAsync(DocenteModel docente, string contrasena);
    Task<IdentityResult> ActualizarAsync(DocenteModel datos);
    Task<bool?> CambiarEstatusAsync(int id);
    Task<IdentityResult> RestablecerContrasenaAsync(int id, string nuevaContrasena);
}