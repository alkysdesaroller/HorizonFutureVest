using HorizonFutureVest.BusinessLogic.DTOs;

namespace HorizonFutureVest.BusinessLogic.Interfaces;

public interface IIndicadorPorPaisService
{
    Task<IEnumerable<IndicadorPorPaisDto>> GetAllAsync();
    Task<IEnumerable<IndicadorPorPaisDto>> GetByFiltersAsync(int? paisId = null, int? year = null);
    Task<IndicadorPorPaisDto> GetByIdAsync(int id);
    Task<IndicadorPorPaisDto> CreateAsync(IndicadorPorPaisDto indicadorDto);
    Task<IndicadorPorPaisDto> UpdateAsync(IndicadorPorPaisDto indicadorDto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExisteCombinacionAsync(int paisId, int macroindicadorId, int year, int? excludeId = null);
    Task<IEnumerable<int>> GetYearsAvalibleAsync();
    Task<IEnumerable<IndicadorPorPaisDto>> GetIndicadoresPorPaisYearAsync(int paisId, int year);
}