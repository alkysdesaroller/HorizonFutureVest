using HorizonFutureVest.BusinessLogic.DTOs;

namespace HorizonFutureVest.BusinessLogic.Interfaces;

public interface IConfiguracionTasasService
{
    Task<ConfiguracionTasasDto> GetConfiguracionTasasAsync();
    Task<ConfiguracionTasasDto> UpdateConfiguracionTasasAsync(ConfiguracionTasasDto configuracionTasasDto);
    Task<(decimal TasaMinima, decimal TasaMaxima)> GetTasasAsync();
}