using HorizonFutureVest.BusinessLogic.DTOs;

namespace HorizonFutureVest.BusinessLogic.Interfaces;

public interface IPaisService
{
    Task<IEnumerable<PaisDto>> GetAllPaisesAsync();
    Task<PaisDto> GetPaisByIdAsync(int id);
    Task<PaisDto> CreatePaisAsync(PaisDto paisDto);
    Task<PaisDto> UpdatePaisAsync(PaisDto paisDto);
    Task<bool> DeletePaisAsync(int id);
    Task<bool> PaisExistsAsync(int id);
    Task<bool> CodigoIsoExistsAsync(string codigoIso, int? excludeId = null);  
}