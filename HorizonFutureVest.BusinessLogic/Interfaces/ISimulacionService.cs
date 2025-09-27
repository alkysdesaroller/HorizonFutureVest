using HorizonFutureVest.BusinessLogic.DTOs;

namespace HorizonFutureVest.BusinessLogic.Interfaces;

public interface ISimulacionService
{
    Task<IEnumerable<MacroIndicadorSimulacionDto>> GetMacroindicadoresSimulacionAsync();
    Task<MacroIndicadorSimulacionDto> GetSimulacionByIdAsync(int id);
    Task<MacroIndicadorSimulacionDto> AgregarMacroindicadorAsync(int macroindicadorId, decimal peso);
    Task<MacroIndicadorSimulacionDto> ActualizarPesoAsync(int id, decimal nuevoPeso);
    Task<bool> EliminarMacroIndicadorAsync(int id);
    Task<decimal> GetSumaPesosSimulacionAsync();
    Task<bool> PuedeAgregarMasAsync();
    Task<bool> ValidarPesoSimulacionAsync(decimal nuevoPeso, int? excludeId = null);
    Task<IEnumerable<MacroIndicadorDto>> GetMacroIndicadoresDisponiblesAsync();
    Task<bool> YaExisteEnSimulacionAsync(int macroindicadorId, int? excludeId = null);
    Task LimpiarSimulacionAsync();
}