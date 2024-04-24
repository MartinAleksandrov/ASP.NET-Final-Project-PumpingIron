namespace Pumping_Iron.Core.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Pumping_Iron.Data.ViewModels.TrainingPrograms;
    using Pumping_Iron.Infrastructure.Extensions;
    using Pumping_Iron.Services.Interfaces;

    public class TrainingProgramController : Controller
    {
        private readonly ITrainingProgramService programService;

        public TrainingProgramController(ITrainingProgramService service)
        {
            programService = service;
        }

        [HttpGet]
        [Authorize(Roles ="Client")]
        public async Task<IActionResult> AllTrainingPrograms()
        {
            var allTrainingPrograms = await programService.AllTrainingProgramsAsync();

            return View(allTrainingPrograms);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            var model = await programService.GetProgramDetailsAsync(Id);

            if (model == null)
            {
                return View("~/Views/Home/Error500.cshtml");
            }

            return View(model);
        }


        [HttpGet]
        [Authorize(Roles = "Coach")]
        public IActionResult CreateProgram()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> CreateProgram(CreateProgramViewModel viewModel)
        {
            var userId = User.GetId();

            try
            {
                var isCreated = await programService.CreateProgramAsync(viewModel, userId);
                if (!isCreated)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create the program. It may already exist or there was an issue with the provided data.");
                    return View(viewModel); // Return the view with the model to display validation errors
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later or contact the administrator.");
                return View(viewModel); // Return the view with the model to display the error message
            }

            return RedirectToAction(nameof(TrainerPrograms));

        }

        [HttpGet]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> TrainerPrograms()
        {
            var userId = User.GetId();

            var trainingPrograms = await programService.GetMyTrainingProgramsAsync(userId);

            if (trainingPrograms == null)
            {
                return View("~/Views/Home/Error500.cshtml");
            }

            return View(trainingPrograms);
        }

    }
}