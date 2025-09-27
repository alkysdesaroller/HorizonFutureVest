using HorizonFutureVest.BusinessLogic.DTOs;
using HorizonFutureVest.BusinessLogic.Interfaces;
using HorizonFutureVest.Persistence.DbContextHorizon;
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

    public Task<MacroIndicadorDto> CreateAsync(MacroIndicadorDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<MacroIndicadorDto> UpdateAsync(MacroIndicadorDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetPesosSumAsync()
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetPesosExceptoAsync(int excludeId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> PuedeCrearNuevoAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidarPesoAsync(decimal nuevoPeso, int? excludeId = null)
    {
        throw new NotImplementedException();
    }
}