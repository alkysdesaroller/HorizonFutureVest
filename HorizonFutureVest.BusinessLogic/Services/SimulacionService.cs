using HorizonFutureVest.BusinessLogic.DTOs;
using HorizonFutureVest.BusinessLogic.Interfaces;
using HorizonFutureVest.Persistence.DbContextHorizon;
using HorizonFutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace HorizonFutureVest.BusinessLogic.Services;

public class SimulacionService(HorizonContext context) : ISimulacionService
{
    public async Task<IEnumerable<MacroIndicadorSimulacionDto>> GetMacroindicadoresSimulacionAsync()
    {
        var simulacion = await context.MacroIndicadorSimulaciones
            .Include(ms => ms.Macroindicador)
            .OrderBy(ms => ms.Macroindicador.Nombre)
            .ToListAsync();
        return simulacion.Select(ms => new MacroIndicadorSimulacionDto
        {
            Id = ms.Id, MacroindicadorId = ms.MacroindicadorId, NombreMacroindicador = ms.Macroindicador.Nombre,
            PesoSimulacion = ms.PesoSimulacion, EsMejorMasAlto = ms.Macroindicador.EsMejorMasAlto
        });
    }

    public async Task<MacroIndicadorSimulacionDto> GetSimulacionByIdAsync(int id)
    {
        var simulacion = await context.MacroIndicadorSimulaciones
            .Include(ms => ms.Macroindicador)
            .FirstOrDefaultAsync(ms => ms.Id == id);
        
        if (simulacion == null) return null!;

        return new MacroIndicadorSimulacionDto
        {
            Id = simulacion.Id,
            MacroindicadorId = simulacion.MacroindicadorId,
            NombreMacroindicador = simulacion.Macroindicador.Nombre,
            PesoSimulacion = simulacion.PesoSimulacion,
            EsMejorMasAlto = simulacion.Macroindicador.EsMejorMasAlto
        };
    }

    public async Task<MacroIndicadorSimulacionDto> AgregarMacroindicadorAsync(int macroindicadorId, decimal peso)
    {
        var macroIndicador = await context.MacroIndicadores.FindAsync(macroindicadorId);
        if (macroIndicador == null) return null!;
        
        var simulacion = new MacroIndicadorSimulacion
        {
            MacroindicadorId = macroindicadorId,
            PesoSimulacion = peso
        };
        context.MacroIndicadorSimulaciones.Add(simulacion);
        await context.SaveChangesAsync();

        return new MacroIndicadorSimulacionDto
        {
            Id = simulacion.Id,
            MacroindicadorId = simulacion.MacroindicadorId,
            NombreMacroindicador = macroIndicador.Nombre,
            PesoSimulacion = simulacion.PesoSimulacion,
            EsMejorMasAlto = macroIndicador.EsMejorMasAlto
        };
    }

    public async Task<MacroIndicadorSimulacionDto> ActualizarPesoAsync(int id, decimal nuevoPeso)
    {
        var simulacion = await context.MacroIndicadorSimulaciones
            .Include(ms => ms.Macroindicador)
            .FirstOrDefaultAsync(ms => ms.Id == id);
        if (simulacion == null) return null!;

        simulacion.PesoSimulacion = nuevoPeso;
        await context.SaveChangesAsync();

        return new MacroIndicadorSimulacionDto
        {
            Id = simulacion.Id,
            MacroindicadorId = simulacion.MacroindicadorId,
            NombreMacroindicador = simulacion.Macroindicador.Nombre,
            PesoSimulacion = simulacion.PesoSimulacion,
            EsMejorMasAlto = simulacion.Macroindicador.EsMejorMasAlto
        };  
    }

    public async Task<bool> EliminarMacroIndicadorAsync(int id)
    {
        var simulacion = await context.MacroIndicadorSimulaciones.FindAsync(id);
        if (simulacion == null) return false;

        context.MacroIndicadorSimulaciones.Remove(simulacion);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<decimal> GetSumaPesosSimulacionAsync()
    {
       return await context.MacroIndicadorSimulaciones.SumAsync(ms => ms.PesoSimulacion);
    }

    public async Task<bool> PuedeAgregarMasAsync()
    {
        var sumaPesos = await GetSumaPesosSimulacionAsync();
            return sumaPesos < 1.0m;
    }

    public async Task<bool> ValidarPesoSimulacionAsync(decimal nuevoPeso, int? excludeId = null)
    {
        var query =  context.MacroIndicadorSimulaciones.AsQueryable();

        if (excludeId.HasValue)
            query = query.Where(ms => ms.Id != excludeId.Value);
        var sumaPesos = await query.SumAsync(ms => ms.PesoSimulacion);
        return (sumaPesos + nuevoPeso) <= 1.0m;
    }

    public async Task<IEnumerable<MacroIndicadorDto>> GetMacroIndicadoresDisponiblesAsync()
    {
        var idSimulacion = await context.MacroIndicadorSimulaciones
            .Select(ms => ms.MacroindicadorId)
            .ToListAsync();
        var disponibles = await context.MacroIndicadores
            .Where(m => !idSimulacion.Contains(m.Id))
            .OrderBy(m => m.Nombre)
            .ToListAsync();

        return disponibles.Select(m => new MacroIndicadorDto
        {
            Id = m.Id,
            Nombre = m.Nombre,
            Peso = m.Peso,
            EsMejorMasAlto = m.EsMejorMasAlto
        });
    }

    public async Task<bool> YaExisteEnSimulacionAsync(int macroindicadorId, int? excludeId = null)
    {
        var query = context.MacroIndicadorSimulaciones
            .Where(ms => ms.MacroindicadorId == macroindicadorId);
        if (excludeId.HasValue)
            query = query.Where(ms => ms.Id != excludeId.Value);
        
        return await query.AnyAsync();
    }

    public Task LimpiarSimulacionAsync()
    {
      var simulacion =  context.MacroIndicadorSimulaciones;
      context.MacroIndicadorSimulaciones.RemoveRange(simulacion);
      return context.SaveChangesAsync();
    }
}