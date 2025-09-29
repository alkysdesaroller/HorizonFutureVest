using HorizonFutureVest.BusinessLogic.Interfaces;
using HorizonFutureVest.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace HorizonFutureVest.Controllers;

public class MacroIndicadorController : Controller
{
    private readonly MacroIndicadorService  _macroIndicadorService;
    private readonly IMacroIndicadorService _indicadorService;
    // GET
    public IActionResult Index()
    {
        return View();
    }
}