using HorizonFutureVest.BusinessLogic.DTOs;
using HorizonFutureVest.BusinessLogic.Interfaces;
using HorizonFutureVest.BusinessLogic.Services;
using HorizonFutureVest.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HorizonFutureVest.Controllers;

public class MacroIndicadorController : Controller
{
    private readonly IMacroIndicadorService _indicadorService;

    public MacroIndicadorController(IMacroIndicadorService indicadorService)
    {
        _indicadorService = indicadorService;
    }

// GET
    public async Task<IActionResult> Index()
    {
        var macroIndicadores = await _indicadorService.GetAllAsync();
        var viewModels = macroIndicadores.Select(m => new MacroIndicadorViewModel
        {
            Id = m.Id,
            Nombre = m.Nombre,
            Peso = m.Peso,
            EsMejorMasAlto = m.EsMejorMasAlto
        }).ToList();
        return View(viewModels);
    }

    public async Task<IActionResult> Create()
    {
        if (!await _indicadorService.PuedeCrearNuevoAsync())
        {
            TempData["Error"] = "No se pudo crear nuevo Macroindicador. La suma de pesos ha alcanzado el maximo (1.0)";
            return RedirectToAction(nameof(Index));
        }

        var viewModel = new MacroIndicadorViewModel
        {
            SumaPesosActual = await _indicadorService.GetPesosSumAsync()
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MacroIndicadorViewModel viewModel)
    {
        viewModel.SumaPesosActual = await _indicadorService.GetPesosSumAsync();

        if (!await _indicadorService.ValidarPesoAsync(viewModel.Peso))
        {
            ModelState.AddModelError(nameof(viewModel.Peso),
                $"El peso excede el limite permitido. Suma actual: {viewModel.SumaPesosActual:P2}, maximo disponible: {(1.0m - viewModel.SumaPesosActual):P2}");
        }

        if (!ModelState.IsValid)
            return View(viewModel);

        var macroIndicadorDto = new MacroIndicadorDto
        {
            Nombre = viewModel.Nombre,
            Peso = viewModel.Peso,
            EsMejorMasAlto = viewModel.EsMejorMasAlto
        };
        await _indicadorService.CreateAsync(macroIndicadorDto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var macroIndicadores = await _indicadorService.GetByIdAsync(id);
        if (macroIndicadores == null!)
            return NotFound();

        var viewModel = new MacroIndicadorViewModel
        {
            Id = macroIndicadores.Id,
            Nombre = macroIndicadores.Nombre,
            Peso = macroIndicadores.Peso,
            EsMejorMasAlto = macroIndicadores.EsMejorMasAlto,
            SumaPesosActual = await _indicadorService.GetPesosExceptoAsync(id)
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(MacroIndicadorViewModel viewModel)
    {
        viewModel.SumaPesosActual = await _indicadorService.GetPesosExceptoAsync(viewModel.Id);

        if (!await _indicadorService.ValidarPesoAsync(viewModel.Peso, viewModel.Id))
        {
            ModelState.AddModelError(nameof(viewModel.Peso),
                 $"El peso excede el límite permitido. Suma actual (sin este indicador): {viewModel.SumaPesosActual:P2}, máximo disponible: {(1.0m - viewModel.SumaPesosActual):P2}");
        }
        
        if(!ModelState.IsValid)
            return View(viewModel);

        var macroIndicadorDto = new MacroIndicadorDto
        {
            Id = viewModel.Id,
            Nombre = viewModel.Nombre,
            Peso = viewModel.Peso,
            EsMejorMasAlto = viewModel.EsMejorMasAlto
        };
        await _indicadorService.UpdateAsync(macroIndicadorDto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var macroIndicadores = await _indicadorService.GetByIdAsync(id);
        if (macroIndicadores == null!)
            return NotFound();

        var viewModel = new MacroIndicadorViewModel
        {
            Id = macroIndicadores.Id,
            Nombre = macroIndicadores.Nombre,
            Peso = macroIndicadores.Peso,
            EsMejorMasAlto = macroIndicadores.EsMejorMasAlto,
        };
        return View(viewModel);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _indicadorService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}