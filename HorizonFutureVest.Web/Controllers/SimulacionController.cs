/*using HorizonFutureVest.BusinessLogic.DTOs;
using HorizonFutureVest.BusinessLogic.Interfaces;
using HorizonFutureVest.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HorizonFutureVest.Controllers;

public class SimulacionController : Controller
{
    private readonly ISimulacionService _simulacionService;
    private readonly IRankingService _rankingService;

    public SimulacionController(ISimulacionService simulacionService, IRankingService rankingService)
    {
        _simulacionService = simulacionService;
        _rankingService = rankingService;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        var macroIndicadoresSimulacion = await _simulacionService.GetMacroindicadoresSimulacionAsync();
        var years = await _rankingService.GetYearConIndicadoresAsync();
        var yearMasReciente = years.Any() ? years.First() : DateTime.Now.Year;
        
        var viewModel = SimulacionViewModel{

            MacroIndicadorSimulacion = macroIndicadoresSimulacion.Select(ms => new MacroIndicadorSimulacionDto
            {
                Id = ms.Id,
                MacroindicadorId = ms.MacroindicadorId,
                NombreMacroindicador = ms.NombreMacroindicador,
                PesoSimulacion = ms.PesoSimulacion,
                EsMejorMasAlto = ms.EsMejorMasAlto
            }).ToList(),
             //   yearSelected = yearMasReciente,
        }
        return View();
    }
}
*/