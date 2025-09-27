using HorizonFutureVest.BusinessLogic.DTOs;

namespace HorizonFutureVest.BusinessLogic.Interfaces;

public interface IMacroIndicadorService
{
    Task<IEnumerable<MacroIndicadorDto>> GetAllAsync();
    Task<MacroIndicadorDto> GetByIdAsync(int id);
    Task<MacroIndicadorDto> CreateAsync(MacroIndicadorDto dto);
    Task<MacroIndicadorDto> UpdateAsync(MacroIndicadorDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<decimal> GetPesosSumAsync();
    Task<decimal> GetPesosExceptoAsync(int excludeId);
    Task<bool> PuedeCrearNuevoAsync();
    Task<bool> ValidarPesoAsync(decimal nuevoPeso, int? excludeId = null);
}