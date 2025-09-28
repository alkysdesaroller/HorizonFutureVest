using HorizonFutureVest.BusinessLogic.DTOs;
using HorizonFutureVest.BusinessLogic.Interfaces;
using HorizonFutureVest.Persistence.DbContextHorizon;
using HorizonFutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace HorizonFutureVest.BusinessLogic.Services;

public class MacroIndicadorService(HorizonContext context) : IMacroIndicadorService
{
    public async Task<IEnumerable<MacroIndicadorDto>> GetAllAsync()
    {
        var macroIndicadores = await context.MacroIndicadores
            .OrderBy(mi => mi.Nombre)
            .ToListAsync();

        return macroIndicadores.Select(mi => new MacroIndicadorDto
        {
            Id = mi.Id,
            Nombre = mi.Nombre,
            Peso = mi.Peso,
            EsMejorMasAlto = mi.EsMejorMasAlto
        });
    }

    public async Task<MacroIndicadorDto> GetByIdAsync(int id)
    {
        var macroIndicador = await context.MacroIndicadores.FindAsync(id);
        if (macroIndicador == null) return null!;

        return new MacroIndicadorDto
        {
            Id = macroIndicador.Id,
            Nombre = macroIndicador.Nombre,
            Peso = macroIndicador.Peso,
            EsMejorMasAlto = macroIndicador.EsMejorMasAlto
        };
    }

    public async Task<MacroIndicadorDto> CreateAsync(MacroIndicadorDto dto)
    {
        var macroIndicador = new MacroIndicador
        {
            Nombre = dto.Nombre,
            Peso = dto.Peso,
            EsMejorMasAlto = dto.EsMejorMasAlto
        };
        context.MacroIndicadores.Add(macroIndicador);
        await context.SaveChangesAsync();

        dto.Id = macroIndicador.Id;
        return dto;
    }

    public async Task<MacroIndicadorDto> UpdateAsync(MacroIndicadorDto dto)
    {
        var macroIndicador = await context.MacroIndicadores.FindAsync(dto.Id);
        if (macroIndicador == null) return null!;
        
        macroIndicador.Nombre = dto.Nombre;
        macroIndicador.Peso = dto.Peso;
        macroIndicador.EsMejorMasAlto = dto.EsMejorMasAlto;

        await context.SaveChangesAsync();
        return dto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
       var macroIndicador = await context.MacroIndicadores.FindAsync(id);
       if (macroIndicador == null) return false;
       
       context.MacroIndicadores.Remove(macroIndicador);
       await context.SaveChangesAsync();
       return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
    return await context.MacroIndicadores.AnyAsync(m => m.Id == id);
    }

    public async Task<decimal> GetPesosSumAsync()
    {
        return await context.MacroIndicadores.SumAsync(m => m.Peso); 
    }

    public async Task<decimal> GetPesosExceptoAsync(int excludeId)
    {
        return await context.MacroIndicadores
            .Where(m => m.Id != excludeId)
            .SumAsync(m => m.Peso);
    }

    public async Task<bool> PuedeCrearNuevoAsync()
    {
        var sumaPesos = await GetPesosSumAsync();
        return sumaPesos < 1.0m;
    }

    public async Task<bool> ValidarPesoAsync(decimal nuevoPeso, int? excludeId = null)
    {
        decimal sumaActual;
        if (excludeId.HasValue)
            sumaActual = await GetPesosExceptoAsync(excludeId.Value);
        else
            sumaActual = await GetPesosSumAsync();
        
        return (sumaActual + nuevoPeso) <= 1.0m;
    }
}