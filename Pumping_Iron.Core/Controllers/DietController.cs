namespace Pumping_Iron.Core.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Pumping_Iron.Services.Interfaces;

    public class DietController : Controller
    {
        private readonly IDietService dietService;

        public DietController(IDietService service)
        {
            dietService = service;
        }

        [HttpGet]
        public async Task<IActionResult> AllDiets()
        {
            var allDiets = await dietService.AllDietsAsync();

            return View(allDiets);
        }
    }
}
