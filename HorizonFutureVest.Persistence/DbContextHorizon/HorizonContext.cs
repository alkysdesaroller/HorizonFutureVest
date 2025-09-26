using HorizonFutureVest.Persistence.Entities;
using HorizonFutureVest.Persistence.EntitiesConfiguration;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HorizonFutureVest.Persistence.DbContextHorizon;

public class HorizonContext : DbContext
{
    public HorizonContext(DbContextOptions<HorizonContext> options) : base(options)
    {
    }
    
    public DbSet<Pais> Paises { get; set; }
    public DbSet<MacroIndicador> MacroIndicadores { get; set; }
    public DbSet<IndicadorPorPais> IndicadoresPorPais { get; set; }
    public DbSet<ConfiguracionTasas> ConfiguracionTasas { get; set; }
    public DbSet<MacroIndicadorSimulacion> MacroIndicadorSimulaciones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());  
        }
    }
