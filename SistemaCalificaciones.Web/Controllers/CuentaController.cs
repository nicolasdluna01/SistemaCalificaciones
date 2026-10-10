using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaCalificaciones.Class.Repositories;
using SistemaCalificaciones.Web.Models;

namespace SistemaCalificaciones.Web.Controllers;

public class CuentaController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IDocenteRepository _docenteRepository;

    public CuentaController(SignInManager<IdentityUser> signInManager, IDocenteRepository docenteRepository)
    {
        _signInManager = signInManager;
        _docenteRepository = docenteRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View(new LoginViewModel());
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // El docente debe existir y estar activo
        var docente = await _docenteRepository.GetByUsuario(model.Usuario);
        if (docente == null || !docente.Estatus)
        {
            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            model.Usuario, model.Contrasena, isPersistent: false, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(string.Empty, "Demasiados intentos fallidos. Intenta de nuevo en unos minutos.");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }
}