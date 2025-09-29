
using HorizonFutureVest.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HorizonFutureVest.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HorizonFutureVest.Controllers;

public class HomeController : Controller
{
    private readonly IRankingService _rankingService;

    public HomeController(IRankingService rankingService)
    {
      
        _rankingService = rankingService;
    }

    public async Task<IActionResult> Index()
    {
        var year = await _rankingService.GetYearConIndicadoresAsync();
        var enumerable = year as int[] ?? year.ToArray();
        var yearMasReciente = enumerable.Any() ? enumerable.First() : DateTime.Now.Year;

        var viewModel = new RankingViewModel
        {
            YearSelected = yearMasReciente,
            YearsAvailable = new SelectList(enumerable, yearMasReciente)
        };
        
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> ObtenerRanking(int yearSelected)
    {
        var years = await _rankingService.GetYearConIndicadoresAsync();
        var viewModel = new RankingViewModel
        {
            YearSelected = yearSelected,
            YearsAvailable = new SelectList(years, yearSelected)
        };
        var (success, errorMessage, resultados) = await _rankingService.GenerarRankingAsync(yearSelected);

        if (!success)
        {
            viewModel.MensajeError = errorMessage;
            return View("Index", viewModel);
        }

        viewModel.Resultados = resultados.Select((r, index) => new RankingResultViewModel
        {
            NombrePais = r.NombrePais,
            CodigoPais = r.CodigoIso,
            Puntaje = r.Scoring,
            TasaRetornoEstimada = r.TasaRetornoEstimada,
            Posicion = index + 1
        }).ToList();
        return View("Index", viewModel);
    }
}