using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaCalificaciones.Class.Models;
using SistemaCalificaciones.Class.Repositories;
using SistemaCalificaciones.Class.Services;
using SistemaCalificaciones.Web.Models;

namespace SistemaCalificaciones.Web.Controllers;

[Authorize(Roles = Roles.Admin)]
public class EstudianteController : Controller
{
    private readonly IEstudianteRepository _repo;
    private readonly IEstudianteService _service;

    public EstudianteController(IEstudianteRepository repo, IEstudianteService service)
    {
        _repo = repo;
        _service = service;
    }

    public async Task<IActionResult> Index(string? buscar)
    {
        var estudiantes = await _repo.GetAll();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            var b = buscar.Trim();
            estudiantes = estudiantes.Where(e =>
                e.Matricula.Contains(b, StringComparison.OrdinalIgnoreCase) ||
                $"{e.Nombre} {e.ApellidoPaterno} {e.ApellidoMaterno}".Contains(b, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        ViewData["Buscar"] = buscar;
        return View(estudiantes
            .OrderBy(e => e.ApellidoPaterno)
            .ThenBy(e => e.ApellidoMaterno)
            .ThenBy(e => e.Nombre));
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new EstudianteViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EstudianteViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var r = await _service.CrearAsync(AEstudiante(vm));
        if (!r.Resultado.Succeeded)
        {
            AgregarErrores(r.Resultado);
            return View(vm);
        }

        // La contraseña temporal se muestra una sola vez en el Index
        TempData["Mensaje"] = "Estudiante creado correctamente.";
        TempData["Usuario"] = vm.Matricula.Trim();
        TempData["Contrasena"] = r.ContrasenaTemporal;
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var e = await _repo.GetById(id);
        if (e == null) return NotFound();

        return View(new EstudianteViewModel
        {
            Id = e.Id,
            Matricula = e.Matricula,
            Nombre = e.Nombre,
            ApellidoPaterno = e.ApellidoPaterno,
            ApellidoMaterno = e.ApellidoMaterno,
            Correo = e.Correo,
            Estatus = e.Estatus
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EstudianteViewModel vm)
    {
        if (id != vm.Id) return NotFound();
        if (!ModelState.IsValid) return View(vm);

        // La matrícula no se modifica: el servicio ignora vm.Matricula
        var result = await _service.ActualizarAsync(AEstudiante(vm));
        if (!result.Succeeded)
        {
            AgregarErrores(result);
            return View(vm);
        }

        TempData["Mensaje"] = "Estudiante actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstatus(int id)
    {
        var estatus = await _service.CambiarEstatusAsync(id);
        if (estatus == null) return NotFound();

        TempData["Mensaje"] = estatus.Value ? "Estudiante activado." : "Estudiante desactivado.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RestablecerContrasena(int id)
    {
        var e = await _repo.GetById(id);
        if (e == null) return NotFound();

        var r = await _service.RestablecerContrasenaAsync(id);
        if (!r.Resultado.Succeeded)
        {
            TempData["Error"] = string.Join(" ", r.Resultado.Errors.Select(x => x.Description));
        }
        else
        {
            TempData["Mensaje"] = "Contraseña restablecida correctamente.";
            TempData["Usuario"] = e.Matricula;
            TempData["Contrasena"] = r.ContrasenaTemporal;
        }
        return RedirectToAction(nameof(Index));
    }

    private static EstudianteModel AEstudiante(EstudianteViewModel vm) => new()
    {
        Id = vm.Id,
        Matricula = vm.Matricula.Trim(),
        Nombre = vm.Nombre.Trim(),
        ApellidoPaterno = vm.ApellidoPaterno.Trim(),
        ApellidoMaterno = vm.ApellidoMaterno.Trim(),
        Correo = vm.Correo.Trim(),
        Estatus = vm.Estatus
    };

    private void AgregarErrores(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }
}