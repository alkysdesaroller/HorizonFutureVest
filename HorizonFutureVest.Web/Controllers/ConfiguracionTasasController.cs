using HorizonFutureVest.BusinessLogic.DTOs;
using HorizonFutureVest.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HorizonFutureVest.Controllers;

public class ConfiguracionTasasController : Controller
{
    private readonly IConfiguracionTasasService _service;
    
    public ConfiguracionTasasController(IConfiguracionTasasService service)
    {
        _service = service;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        var config = await _service.GetConfiguracionTasasAsync();
        
        var viewModel = new ViewModels.ConfiguracionTasasViewModel
        {
            Id = config.Id,
            TasaMinima = config.TasaMinima,
            TasaMaxima = config.TasaMaxima
        };
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ViewModels.ConfiguracionTasasViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var configuracionDto = new ConfiguracionTasasDto
        {
            Id = model.Id,
            TasaMinima = model.TasaMinima,
            TasaMaxima = model.TasaMaxima
        };
        await _service.UpdateConfiguracionTasasAsync(configuracionDto);
        
        TempData["Success"] = "Configuracion de tasas actualizada exitosamente.";
        return View(model);
    }
    
}