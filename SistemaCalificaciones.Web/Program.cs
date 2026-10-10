using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using SistemaCalificaciones.Class.Data;
using SistemaCalificaciones.Class.Repositories;
using SistemaCalificaciones.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Toda la aplicación exige sesión iniciada, salvo lo marcado con [AllowAnonymous]
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Falta la cadena de conexión 'DefaultConnection'. Revisa appsettings.Development.json.");

builder.Services.AddDbContext<SistemaCalificacionesDB>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("SistemaCalificaciones.Class")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
})
.AddEntityFrameworkStores<SistemaCalificacionesDB>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Cuenta/Login";
    options.ExpireTimeSpan = TimeSpan.FromHours(2);
    options.SlidingExpiration = true;
});

builder.Services.AddScoped<IDocenteRepository, DocenteRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Docente de prueba, solo en Desarrollo
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    await DbInitializer.SeedAsync(scope.ServiceProvider);
}

app.Run();
