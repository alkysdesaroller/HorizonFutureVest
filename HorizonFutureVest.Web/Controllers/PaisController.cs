using HorizonFutureVest.BusinessLogic.Interfaces;
using HorizonFutureVest.BusinessLogic.Services;
using HorizonFutureVest.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HorizonFutureVest.Controllers;

public class PaisController : Controller
{
    private readonly PaisService  _service;
   

    public PaisController(PaisService service)
    {
        _service = service;
    }

    // GET
    public IActionResult Index()
    {
        var viewModel = new PaisViewModel();
        return View(viewModel);
    }
    
}