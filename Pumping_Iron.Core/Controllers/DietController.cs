namespace Pumping_Iron.Core.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Pumping_Iron.Data.ViewModels.Diet;
    using Pumping_Iron.Data.ViewModels.TrainingPrograms;
    using Pumping_Iron.Infrastructure.Extensions;
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

        [HttpGet]
        [Authorize(Roles = "Coach")]
        public IActionResult CreateDiet()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> CreateDiet(CreateDietViewModel viewModel)
        {
            var userId = User.GetId();

            try
            {
                var isCreated = await dietService.CreateDietAsync(viewModel, userId);
                if (!isCreated)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create the diet. It may already exist or there was an issue with the provided data.");
                    return View(viewModel); // Return the view with the model to display validation errors
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later or contact the administrator.");
                return View(viewModel); // Return the view with the model to display the error message
            }

            return RedirectToAction(nameof(AllDiets));

        }
    }
}
