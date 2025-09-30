using HorizonFutureVest.BusinessLogic.DTOs;
using HorizonFutureVest.BusinessLogic.Interfaces;
using HorizonFutureVest.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

    // GET: /Simulacion
    public async Task<IActionResult> Index()
    {
        var macroIndicadoresSimulacion = await _simulacionService.GetMacroindicadoresSimulacionAsync();
        var years = await _rankingService.GetYearConIndicadoresAsync();
        var yearMasReciente = years.Any() ? years.First() : DateTime.Now.Year;

        var viewModel = new SimulacionViewModel
        {
            MacroIndicadoresSimulacion = macroIndicadoresSimulacion.Select(m => new MacroIndicadorSimulacionViewModel
            {
                Id = m.Id,
                NombreMacroindicador = m.NombreMacroindicador,
                PesoSimulacion = m.PesoSimulacion,
                EsMejorMasAlto = m.EsMejorMasAlto,
            }).ToList(),
            YearSelected = yearMasReciente,
            YearsAvailable = new SelectList(years, yearMasReciente),
            SumaPesosActual = await _simulacionService.GetSumaPesosSimulacionAsync(),
            PuedeAgregarMas = await _simulacionService.PuedeAgregarMasAsync()
        };
        return View(viewModel);
    }

    public async Task<IActionResult> Agregar()
    {
        if (!await _simulacionService.PuedeAgregarMasAsync())
        {
            TempData["Error"] = "No se pudo agregar nuevo Macroindicador. La suma de pesos ha alcanzado el máximo (1.0)";
            return RedirectToAction(nameof(Index));
        }

        var macroIndicadoresDisponibles = await _simulacionService.GetMacroIndicadoresDisponiblesAsync();

        var viewModel = new MacroIndicadorSimulacionViewModel
        {
            MacroindicadoresDisponibles = new SelectList(macroIndicadoresDisponibles, "Id", "Nombre")
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Agregar(MacroIndicadorSimulacionViewModel model)
    {
        if (!await _simulacionService.ValidarPesoSimulacionAsync(model.NuevoPeso))
        {
            ModelState.AddModelError(nameof(model.NuevoPeso),
                "El peso excede el límite permitido para la simulación.");
        }

        if (!ModelState.IsValid)
        {
            var macroIndicadoresDisponibles = await _simulacionService.GetMacroIndicadoresDisponiblesAsync();
            model.MacroindicadoresDisponibles = new SelectList(macroIndicadoresDisponibles, "Id", "Nombre");
            return View(model);
        }

        await _simulacionService.AgregarMacroindicadorAsync(model.MacroindicadorSeleccionadoId, model.NuevoPeso);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> EditarPeso(int id)
    {
        var simulacion = await _simulacionService.GetSimulacionByIdAsync(id);
        if (simulacion == null)
            return NotFound();

        var viewModel = new MacroIndicadorSimulacionViewModel
        {
            Id = simulacion.Id,
            NombreMacroindicador = simulacion.NombreMacroindicador,
            NuevoPeso = simulacion.PesoSimulacion,
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarPeso(MacroIndicadorSimulacionViewModel model)
    {
        if (!await _simulacionService.ValidarPesoSimulacionAsync(model.NuevoPeso, model.Id))
        {
            ModelState.AddModelError(nameof(model.NuevoPeso),
                "El peso excede el límite permitido para la simulación.");
        }

        if (!ModelState.IsValid)
            return View(model);

        await _simulacionService.ActualizarPesoAsync(model.Id, model.NuevoPeso);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> SimularRanked(int YearSelected)
    {
        var (success, errorMessage, resultado) = await _rankingService.GenerarRankingAsync(YearSelected);

        if (!success)
        {
            TempData["Error"] = errorMessage;
            return RedirectToAction(nameof(Index));
        }

        var viewModel = new ResultadosSimulacionViewModel
        {
            Year = YearSelected,
            Resultados = resultado.Select(r => new RankingResultViewModel
            {
                NombrePais = r.NombrePais,
                CodigoPais = r.CodigoIso,         // DTO -> VM
                Puntaje = r.Scoring,              // DTO -> VM
                TasaRetornoEstimada = r.TasaRetornoEstimada,
                Posicion = r.Posicion
            }).ToList()
        };

        return View("ResultadosSimulacion", viewModel);
    }

    public async Task<IActionResult> ResultadosSimulacion(int year)
    {
        // Si llamas directo a esta acción, sin TempData, podrías cargar de DB
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> LimpiarSimulacionAsync()
    {
        await _simulacionService.LimpiarSimulacionAsync();
        TempData["Success"] = "La simulación ha sido limpiada correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
