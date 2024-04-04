namespace Pumping_Iron.Core.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Pumping_Iron.Services.Interfaces;

    [Authorize(Roles = "Coach")]
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

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await dietService.GetDetaisByIdAsync(id);

            if (model == null)
            {
                return BadRequest();
            }

            return View(model);
        }
    }
}
