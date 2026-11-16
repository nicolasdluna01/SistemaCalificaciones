using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaCalificaciones.Class.Models;
using SistemaCalificaciones.Class.Repositories;

namespace SistemaCalificaciones.Web.Controllers;

[Authorize(Roles = Roles.Admin)]
public class AsignaturaController : Controller
{
    private readonly IAsignaturaRepository _repo;

    public AsignaturaController(IAsignaturaRepository repo)
    {
        _repo = repo;
    }

    public async Task<IActionResult> Index()
    {
        var asignaturas = (await _repo.GetAll())
            .OrderBy(a => a.Semestre)
            .ThenBy(a => a.NombreAsignatura);
        return View(asignaturas);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new AsignaturaModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("ClaveAsignatura,NombreAsignatura,Creditos,Semestre,Unidades,Estatus")] AsignaturaModel model)
    {
        await ValidarClave(model);
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _repo.Create(model);
        TempData["Mensaje"] = "Asignatura creada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var asignatura = await _repo.GetById(id);
        if (asignatura == null) return NotFound();
        return View(asignatura);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,ClaveAsignatura,NombreAsignatura,Creditos,Semestre,Unidades,Estatus")] AsignaturaModel model)
    {
        if (id != model.Id) return NotFound();

        await ValidarClave(model);
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _repo.Update(model);
        TempData["Mensaje"] = "Asignatura actualizada correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstatus(int id)
    {
        var asignatura = await _repo.GetById(id);
        if (asignatura == null) return NotFound();

        asignatura.Estatus = !asignatura.Estatus;
        await _repo.Update(asignatura);

        TempData["Mensaje"] = asignatura.Estatus ? "Asignatura activada." : "Asignatura desactivada.";
        return RedirectToAction(nameof(Index));
    }

    private async Task ValidarClave(AsignaturaModel model)
    {
        model.ClaveAsignatura = model.ClaveAsignatura?.Trim().ToUpper()!;

        if (!string.IsNullOrEmpty(model.ClaveAsignatura)
            && await _repo.ExisteClave(model.ClaveAsignatura, model.Id))
        {
            ModelState.AddModelError(nameof(model.ClaveAsignatura), "Ya existe una asignatura con esa clave.");
        }
    }
}