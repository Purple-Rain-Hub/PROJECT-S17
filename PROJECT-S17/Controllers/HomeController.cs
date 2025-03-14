using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PROJECT_S17.Models;
using PROJECT_S17.Services;
using PROJECT_S17.ViewModels;

namespace PROJECT_S17.Controllers
{
    public class HomeController : Controller
    {
        private readonly VerbaleService _verbaleService;

        public HomeController(VerbaleService verbaleService)
        {
            _verbaleService = verbaleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddVerbalePage() 
        {
            List<Violazione> violazioniList = await _verbaleService.GetViolazioni();
            ViewData["Violazioni"] = violazioniList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveVerbale(AddVerbaleViewModel addVerbaleViewModel)
        {
            await _verbaleService.SaveVerbaleAsync(addVerbaleViewModel);
            return RedirectToAction("Index");
        }
    }
}
