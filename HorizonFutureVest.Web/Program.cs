using HorizonFutureVest.BusinessLogic.Interfaces;
using HorizonFutureVest.BusinessLogic.Services;
using HorizonFutureVest.Persistence.DbContextHorizon;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HorizonContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IPaisService, PaisService>();
builder.Services.AddScoped<IMacroIndicadorService, MacroIndicadorService>();
builder.Services.AddScoped<IIndicadorPorPaisService, IndicadorPorPaisService>();
builder.Services.AddScoped<IRankingService, RankingService>();
builder.Services.AddScoped<IConfiguracionTasasService, ConfiguracionTasasService>();
builder.Services.AddScoped<ISimulacionService, SimulacionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();