using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaCalificaciones.Class.Models;
using SistemaCalificaciones.Class.Repositories;
using SistemaCalificaciones.Class.Services;
using SistemaCalificaciones.Web.Models;

namespace SistemaCalificaciones.Web.Controllers;

[Authorize(Roles = Roles.Admin)]
public class DocenteController : Controller
{
    private readonly IDocenteRepository _repo;
    private readonly IDocenteService _service;

    public DocenteController(IDocenteRepository repo, IDocenteService service)
    {
        _repo = repo;
        _service = service;
    }

    public async Task<IActionResult> Index()
    {
        var docentes = (await _repo.GetAll())
            .OrderBy(d => d.ApellidoPaterno)
            .ThenBy(d => d.ApellidoMaterno)
            .ThenBy(d => d.Nombre);
        return View(docentes);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new DocenteViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DocenteViewModel vm)
    {
        if (string.IsNullOrWhiteSpace(vm.Contrasena))
        {
            ModelState.AddModelError(nameof(vm.Contrasena), "La contraseña es obligatoria.");
        }
        if (!ModelState.IsValid) return View(vm);

        var result = await _service.CrearAsync(ADocente(vm), vm.Contrasena!);
        if (!result.Succeeded)
        {
            AgregarErrores(result);
            return View(vm);
        }

        TempData["Mensaje"] = "Docente creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var d = await _repo.GetById(id);
        if (d == null) return NotFound();

        return View(new DocenteViewModel
        {
            Id = d.Id,
            NumeroEmpleado = d.NumeroEmpleado,
            Nombre = d.Nombre,
            ApellidoPaterno = d.ApellidoPaterno,
            ApellidoMaterno = d.ApellidoMaterno,
            Correo = d.Correo,
            Usuario = d.Usuario,
            Estatus = d.Estatus
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DocenteViewModel vm)
    {
        if (id != vm.Id) return NotFound();
        if (!ModelState.IsValid) return View(vm);

        // El usuario no se modifica: el servicio ignora vm.Usuario
        var result = await _service.ActualizarAsync(ADocente(vm));
        if (!result.Succeeded)
        {
            AgregarErrores(result);
            return View(vm);
        }

        TempData["Mensaje"] = "Docente actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstatus(int id)
    {
        var estatus = await _service.CambiarEstatusAsync(id);
        if (estatus == null) return NotFound();

        TempData["Mensaje"] = estatus.Value ? "Docente activado." : "Docente desactivado.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> RestablecerContrasena(int id)
    {
        var d = await _repo.GetById(id);
        if (d == null) return NotFound();

        return View(new RestablecerContrasenaViewModel
        {
            Id = d.Id,
            NombreDocente = $"{d.Nombre} {d.ApellidoPaterno} {d.ApellidoMaterno}"
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RestablecerContrasena(RestablecerContrasenaViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var result = await _service.RestablecerContrasenaAsync(vm.Id, vm.NuevaContrasena);
        if (!result.Succeeded)
        {
            AgregarErrores(result);
            return View(vm);
        }

        TempData["Mensaje"] = "Contraseña restablecida correctamente.";
        return RedirectToAction(nameof(Index));
    }

    private static DocenteModel ADocente(DocenteViewModel vm) => new()
    {
        Id = vm.Id,
        NumeroEmpleado = vm.NumeroEmpleado,
        Nombre = vm.Nombre.Trim(),
        ApellidoPaterno = vm.ApellidoPaterno.Trim(),
        ApellidoMaterno = vm.ApellidoMaterno.Trim(),
        Correo = vm.Correo.Trim(),
        Usuario = vm.Usuario.Trim(),
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
