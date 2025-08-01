using DeliveryLocal.Data;
using Microsoft.EntityFrameworkCore;
using DeliveryLocal.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.MapDefaultControllerRoute();

// Crear admin si no existe
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.Usuarios.Any())
    {
        db.Usuarios.Add(new Usuario { Username = "admin", Password = "saistroy" });
        db.SaveChanges();
    }
}

app.Run();
