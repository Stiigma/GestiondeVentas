using GV.Application.Servicios;
using GV.Domain.Models;
using GV.Domain.Repositorys;
using GV.Infrastructure.Data;
using GV.Infrastructure.Repositorys;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer(); // <-- importante
builder.Services.AddSwaggerGen();

// Adding DB context
builder.Services.AddDbContext<ContextDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConectionDB")));

builder.Services.AddScoped<ContextDB>();
builder.Services.AddScoped<IIngresoRepository, IngresoRepository>();
builder.Services.AddScoped<IPersonaRepository, PersonaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IArticuloRepository, ArticulosRepository>();
builder.Services.AddScoped<ICategoriaRespository, CategoriaRepository>();
builder.Services.AddScoped<IDetalleIngreso, DetalleIngresoRepository>();
builder.Services.AddScoped<IVentasRepository, VentasRepository>();
builder.Services.AddScoped<IDetalleVentas, DetalleVentasRepository>();
builder.Services.AddScoped<ServicioIngresos>();
builder.Services.AddScoped<ServicioVentas>();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Ingresos v1");
    c.RoutePrefix = "swagger"; // URL será: /swagger
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

//app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
