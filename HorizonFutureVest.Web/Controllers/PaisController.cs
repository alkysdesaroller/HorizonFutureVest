using HorizonFutureVest.BusinessLogic.DTOs;
using HorizonFutureVest.BusinessLogic.Interfaces;
using HorizonFutureVest.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HorizonFutureVest.Controllers;

public class PaisController : Controller
{
    private readonly IPaisService  _service;
    
    public PaisController(IPaisService service)
    {
        _service = service;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var paises = await _service.GetAllPaisesAsync();
        var viewModel = paises.Select(p => new PaisViewModel
        {
            Id = p.Id,
            Nombre = p.Nombre,
            CodigoIso = p.CodigoIso
        }).ToList();
        
        return View(viewModel);
    }

    public IActionResult Create()
    {
        return View(new PaisViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PaisViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        if (await _service.CodigoIsoExistsAsync(model.CodigoIso))
        {
            ModelState.AddModelError(nameof(model.CodigoIso), "Ya existe un pais con este codigo ISO");
                return View(model);
        }

        var paisDto = new PaisDto
        {
            Nombre = model.Nombre,
            CodigoIso = model.CodigoIso
        };
        await _service.CreatePaisAsync(paisDto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var pais = await _service.GetPaisByIdAsync(id);
        if (pais == null!)
            return NotFound();

        var viewModel = new PaisViewModel
        {
            Id = pais.Id,
            Nombre = pais.Nombre,
            CodigoIso = pais.CodigoIso
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PaisViewModel model)
    {
        if  (!ModelState.IsValid)
        {
            return View(model);
        }

        if (await _service.CodigoIsoExistsAsync(model.CodigoIso, model.Id))
        {
            ModelState.AddModelError(nameof(model.CodigoIso), "Ya existe un pais con este codigo ISO");
            return View(model);
        }

        var paisDto = new PaisDto
        {
            Id = model.Id,
            Nombre = model.Nombre,
            CodigoIso = model.CodigoIso
        };
        await _service.UpdatePaisAsync(paisDto);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _service.DeletePaisAsync(id);
        return RedirectToAction(nameof(Index));
    }
}