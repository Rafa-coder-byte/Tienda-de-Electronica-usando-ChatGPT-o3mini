using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using mi_tienda_de_electrónica.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

var builder = WebApplication.CreateBuilder(args);


// Agregar servicios MVC y la DB en memoria (para demo)
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar autenticación con cookies
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Admin/Login";
        options.LogoutPath = "/Admin/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20); // La sesión expira después de 20 min de inactividad
        options.SlidingExpiration = false; // No renueva la cookie automáticamente
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}



// Servir archivos estáticos desde wwwroot
app.UseStaticFiles();

app.UseRouting();

// Activar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Rutas convencionales para MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// (Opcional) Inicializar la base de datos con algunos productos de ejemplo
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    try { 
    var db = service.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
        
        
        if (!db.Products.Any())
    {
        db.Products.AddRange(
            new mi_tienda_de_electrónica.Models.Product {
                Name = "Smartphone X",
                Description = "Un smartphone de alta gama",
                Price = 699,
                ImageUrl = "https://via.placeholder.com/250x150?text=Smartphone+X"
            },
            new mi_tienda_de_electrónica.Models.Product {
                Name = "Laptop Pro",
                Description = "Laptop potente para profesionales",
                Price = 1299,
                ImageUrl = "https://via.placeholder.com/250x150?text=Laptop+Pro"
            },
            new mi_tienda_de_electrónica.Models.Product {
                Name = "Auriculares Bluetooth",
                Description = "Auriculares inalámbricos de alta calidad",
                Price = 199,
                ImageUrl = "https://via.placeholder.com/250x150?text=Auriculares+Bluetooth"
            }
        );
        db.SaveChanges();
    }
    }
    
    catch (Exception ex)
    {
        var logger = service.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocurrió un error al inicializar la base de datos.");
    }
}



app.Run();
