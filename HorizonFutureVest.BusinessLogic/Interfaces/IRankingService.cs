using HorizonFutureVest.BusinessLogic.DTOs;

namespace HorizonFutureVest.BusinessLogic.Interfaces;

public interface IRankingService
{
    Task<(bool Success, string ErrorMessage, List<RankingResultDto> Resultado)> GenerarRankingAsync(int year);
    Task<(bool Success, string ErrorMessage, List<RankingResultDto> Resultado)> GenerarRankingSimulacionAsync(int year);
    Task<IEnumerable<int>> GetYearConIndicadoresAsync();
    Task<int> GetYearMasRecienteAsync();
}