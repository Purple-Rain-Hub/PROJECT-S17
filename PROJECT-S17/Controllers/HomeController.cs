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

        public async Task<IActionResult> Index(int selectFilter)
        {
            var verbaliList = new IndexViewModel();
            switch (selectFilter)
            {
                case 0:
                    verbaliList = await _verbaleService.GetAllVerbali();
                    break;
                case 1:
                    verbaliList = await _verbaleService.contestabiliFilter();
                    break;
                case 2:
                    verbaliList = await _verbaleService.DecurtazioniFilter();
                    break;
                case 3:
                    verbaliList = await _verbaleService.ImportoFilter();
                    break;
            }
            return View(verbaliList);
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
            ModelState.Remove("Verbale.Violazione.Descrizione");
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Errore nel modello del form";
                return RedirectToAction("AddVerbalePage");
            }
            await _verbaleService.SaveVerbaleAsync(addVerbaleViewModel);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> TotVerbaliPage()
        {
            var totVerbali = await _verbaleService.GetTotVerbaliAsync();
            return View(totVerbali);
        }

        public async Task<IActionResult> TotDecurtazioniPage()
        {
            var totDecurtazioni = await _verbaleService.GetTotDecurtazioniAsync();
            return View(totDecurtazioni);
        }
    }
}
