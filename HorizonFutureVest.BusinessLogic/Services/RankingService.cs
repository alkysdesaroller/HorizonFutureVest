using HorizonFutureVest.BusinessLogic.DTOs;
using HorizonFutureVest.BusinessLogic.Interfaces;
using HorizonFutureVest.Persistence.DbContextHorizon;
using HorizonFutureVest.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace HorizonFutureVest.BusinessLogic.Services;

public class RankingService(HorizonContext context) : IRankingService
{
    public async Task<(bool Success, string ErrorMessage, List<RankingResultDto> Resultado)> GenerarRankingAsync(int year)
    {
        var macroIndicadores = await context.MacroIndicadores.ToListAsync();
        var sumaPesos = macroIndicadores.Sum(mi => mi.Peso);
        
        if(Math.Abs(sumaPesos - 1.0m) > 0.0001m)
        {
            return (false, "Se deben ajustar los pesos de los macroindicadores registrados hasta que la suma de los mismos sea igual a 1", null!);
        }
        var paisesElegibles = await GetPaisesElegiblesAsync(year, macroIndicadores);
        throw new NotImplementedException();
    }

    private async Task<object> GetPaisesElegiblesAsync(int year, List<MacroIndicador> macroIndicadores)
    {
        throw new NotImplementedException();
    }

    public Task<(bool Success, string ErrorMessage, List<RankingResultDto> Resultado)> GenerarRankingSimulacionAsync(int year)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<int>> GetYearConIndicadoresAsync()
    {
        throw new NotImplementedException();
    }

    public Task<int> GetYearMasRecienteAsync()
    {
        throw new NotImplementedException();
    }
}