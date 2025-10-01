using HorizonFutureVest.BusinessLogic.DTOs;
using HorizonFutureVest.BusinessLogic.Interfaces;
using HorizonFutureVest.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HorizonFutureVest.Controllers;

public class IndicadorPorPaisController : Controller
{
    private readonly IIndicadorPorPaisService _indicadorPorPaisService;
    private readonly IPaisService _paisService;
    private readonly IMacroIndicadorService _macroIndicadorService;

    public IndicadorPorPaisController(IIndicadorPorPaisService indicadorPorPaisService, IPaisService paisService,
        IMacroIndicadorService macroIndicadorService)
    {
        _indicadorPorPaisService = indicadorPorPaisService;
        _paisService = paisService;
        _macroIndicadorService = macroIndicadorService;
    }

    // GET
    public async Task<IActionResult> Index(int? paisId, int? year)
    {
        var indicadores = await _indicadorPorPaisService.GetByFiltersAsync(paisId, year);
        var paises = await _paisService.GetAllPaisesAsync();

        var viewModel = new IndicadorPorPaisViewModel
        {
            FiltrarPorPaisId = paisId,
            FiltrarPorYear = year,
            PaisesFiltro = new SelectList(paises, "Id", "Nombre"),
        };
        ViewBag.Indicadores = indicadores.Select(i => new IndicadorPorPaisViewModel
        {
            Id = i.Id,
            NombrePais = i.NombrePais,
            NombreMacroindicador = i.NombreMacroindicador,
            Valor = i.Valor,
            Year = i.Year,
            PaisId = i.PaisId,
            MacroindicadorId = i.MacroindicadorId
        }).ToList();

        return View(viewModel);
    }

    public async Task<IActionResult> Create()
    {
        var paises = await _paisService.GetAllPaisesAsync();
        var macroindicadores = await _macroIndicadorService.GetAllAsync();

        var viewModel = new IndicadorPorPaisViewModel
        {
            Paises = new SelectList(paises, "Id", "Nombre"),
            Macroindicadores = new SelectList(macroindicadores, "Id", "Nombre"),
            Year = DateTime.Now.Year
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IndicadorPorPaisViewModel viewModel)
    {
        if (await _indicadorPorPaisService.ExisteCombinacionAsync(viewModel.PaisId, viewModel.MacroindicadorId, viewModel.Year))
        {
            ModelState.AddModelError(string.Empty, "Ya existe un indicador para el país, macroindicador y año seleccionados.");
        }

        if (!ModelState.IsValid)  
        {
            var paises = await _paisService.GetAllPaisesAsync();
            var macroindicadores = await _macroIndicadorService.GetAllAsync();
            viewModel.Paises = new SelectList(paises, "Id", "Nombre", viewModel.PaisId);
            viewModel.Macroindicadores = new SelectList(macroindicadores, "Id", "Nombre", viewModel.MacroindicadorId);
            return View(viewModel);
        }

        var indicadorDto = new IndicadorPorPaisDto
        {
            PaisId = viewModel.PaisId,
            MacroindicadorId = viewModel.MacroindicadorId,
            Valor = viewModel.Valor,
            Year = viewModel.Year
        };
        await _indicadorPorPaisService.CreateAsync(indicadorDto);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id)
    {
        var indicador = await _indicadorPorPaisService.GetByIdAsync(id);
        if (indicador == null!)
            return NotFound();
        
        var paises = await _paisService.GetAllPaisesAsync();
        var macroindicadores = await _macroIndicadorService.GetAllAsync();

        var viewModel = new IndicadorPorPaisViewModel
        {
            Id = indicador.Id,
            PaisId = indicador.PaisId,
            MacroindicadorId = indicador.MacroindicadorId,
            Valor = indicador.Valor,
            Year = indicador.Year,
            Paises = new SelectList(paises, "Id", "Nombre", indicador.PaisId),
            Macroindicadores = new SelectList(macroindicadores, "Id", "Nombre", indicador.MacroindicadorId)
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, IndicadorPorPaisViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            var paises = await _paisService.GetAllPaisesAsync();
            var macroindicadores = await _macroIndicadorService.GetAllAsync();
            viewModel.Paises = new SelectList(paises, "Id", "Nombre", viewModel.PaisId);
            viewModel.Macroindicadores = new SelectList(macroindicadores, "Id", "Nombre", viewModel.MacroindicadorId);
            return View(viewModel);
        }
        var indicadorDto = new IndicadorPorPaisDto
        {
            Id = id,
            PaisId = viewModel.PaisId,
            MacroindicadorId = viewModel.MacroindicadorId,
            Valor = viewModel.Valor,
            Year = viewModel.Year
        };
        await _indicadorPorPaisService.UpdateAsync(indicadorDto);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id)
    {
        var indicador = await _indicadorPorPaisService.GetByIdAsync(id);
        if (indicador == null!)
            return NotFound();

        var viewModel = new IndicadorPorPaisViewModel
        {
            Id = indicador.Id,
            NombrePais = indicador.NombrePais,
            NombreMacroindicador = indicador.NombreMacroindicador,
            Valor = indicador.Valor,
            Year = indicador.Year
        };
        return View(viewModel);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _indicadorPorPaisService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}