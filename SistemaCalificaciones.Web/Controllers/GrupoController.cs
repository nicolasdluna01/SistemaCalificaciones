using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaCalificaciones.Class.Models;
using SistemaCalificaciones.Class.Repositories;

namespace SistemaCalificaciones.Web.Controllers;

[Authorize(Roles = Roles.Admin)]
public class GrupoController : Controller
{
    private static readonly string[] Turnos = { "Matutino", "Vespertino", "Nocturno" };

    private readonly IGrupoRepository _repo;

    public GrupoController(IGrupoRepository repo)
    {
        _repo = repo;
    }

    public async Task<IActionResult> Index()
    {
        var grupos = (await _repo.GetAll())
            .OrderByDescending(g => g.Periodo)
            .ThenBy(g => g.Semestre)
            .ThenBy(g => g.NombreGrupo);
        return View(grupos);
    }

    [HttpGet]
    public IActionResult Create()
    {
        CargarTurnos();
        return View(new GrupoModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("NombreGrupo,Semestre,Periodo,Turno,Estatus")] GrupoModel model)
    {
        await Validar(model);
        if (!ModelState.IsValid)
        {
            CargarTurnos();
            return View(model);
        }

        await _repo.Create(model);
        TempData["Mensaje"] = "Grupo creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var grupo = await _repo.GetById(id);
        if (grupo == null) return NotFound();

        CargarTurnos();
        return View(grupo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,NombreGrupo,Semestre,Periodo,Turno,Estatus")] GrupoModel model)
    {
        if (id != model.Id) return NotFound();

        await Validar(model);
        if (!ModelState.IsValid)
        {
            CargarTurnos();
            return View(model);
        }

        await _repo.Update(model);
        TempData["Mensaje"] = "Grupo actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstatus(int id)
    {
        var grupo = await _repo.GetById(id);
        if (grupo == null) return NotFound();

        grupo.Estatus = !grupo.Estatus;
        await _repo.Update(grupo);

        TempData["Mensaje"] = grupo.Estatus ? "Grupo activado." : "Grupo desactivado.";
        return RedirectToAction(nameof(Index));
    }

    private void CargarTurnos()
    {
        ViewBag.Turnos = new SelectList(Turnos);
    }

    private async Task Validar(GrupoModel model)
    {
        model.NombreGrupo = model.NombreGrupo?.Trim().ToUpper()!;
        model.Periodo = model.Periodo?.Trim()!;

        if (!string.IsNullOrEmpty(model.Turno) && !Turnos.Contains(model.Turno))
        {
            ModelState.AddModelError(nameof(model.Turno), "Selecciona un turno válido.");
        }

        if (!string.IsNullOrEmpty(model.NombreGrupo) && !string.IsNullOrEmpty(model.Periodo)
            && await _repo.ExisteGrupo(model.NombreGrupo, model.Periodo, model.Id))
        {
            ModelState.AddModelError(string.Empty, "Ya existe un grupo con ese nombre en ese periodo.");
        }
    }
}