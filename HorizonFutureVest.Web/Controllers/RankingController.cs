using Microsoft.AspNetCore.Mvc;

namespace HorizonFutureVest.Controllers;

public class RankingController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}