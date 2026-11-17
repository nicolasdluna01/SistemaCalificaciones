using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaCalificaciones.Class.Models;
using SistemaCalificaciones.Class.Repositories;
using SistemaCalificaciones.Web.Models;

namespace SistemaCalificaciones.Web.Controllers;

public class CuentaController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IDocenteRepository _docenteRepository;
    private readonly IEstudianteRepository _estudianteRepository;

    public CuentaController(
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IDocenteRepository docenteRepository,
        IEstudianteRepository estudianteRepository)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _docenteRepository = docenteRepository;
        _estudianteRepository = estudianteRepository;
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

        // Un docente dado de baja no puede entrar aunque su contraseña sea correcta
        var user = await _userManager.FindByNameAsync(model.Usuario);
        if (user != null && await _userManager.IsInRoleAsync(user, Roles.Docente))
        {
            var docente = await _docenteRepository.GetByUsuario(model.Usuario);
            if (docente == null || !docente.Estatus)
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View(model);
            }
        }

        // Un alumno dado de baja no puede entrar aunque su contraseña sea correcta
        if (user != null && await _userManager.IsInRoleAsync(user, Roles.Alumno))
        {
            var estudiante = await _estudianteRepository.GetByMatricula(model.Usuario);
            if (estudiante == null || !estudiante.Estatus)
            {
                ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                return View(model);
            }
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

    [AllowAnonymous]
    public IActionResult AccesoDenegado()
    {
        return View();
    }
}