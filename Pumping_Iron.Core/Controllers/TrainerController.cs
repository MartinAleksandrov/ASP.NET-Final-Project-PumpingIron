namespace Pumping_Iron.Core.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Pumping_Iron.Infrastructure.Extensions;
    using Pumping_Iron.Services;
    using Pumping_Iron.Services.Interfaces;

    public class TrainerController : Controller
    {
        private readonly ITrainerService trainerService;

        public TrainerController(ITrainerService service)
        {
            trainerService = service;
        }

        [Authorize(Roles = "Client")]
        [HttpGet]
        public async Task<IActionResult> AllTrainers()
        {
            var allTrainers = await trainerService.AllTrainersAsync();

            return View(allTrainers);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var model = await trainerService.GetTrainerDetailsAsync(id);

            if (model == null)
            {
                return View("~/Views/Home/Error500.cshtml");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> AddProgram(string id)
        {
            // Get the current trainer's ID
            var trainerId = User.GetId();

            // Call the service to retrieve all programs associated with the trainer and client
            var model = await trainerService.GetAllTrainerPrograms(trainerId, id);

            // If no programs are found for the trainer and client, return an empty view
            if (model == null)
            {
                // If no programs are found, return a view without any model data
                return View();
            }

            // If programs are found, return the view with the model data
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> AddProgramToClient(int programId, string clientId)
        {
            try
            {
                var addProgram = await trainerService.AddProgramToClient(programId, clientId);

                if (addProgram)
                {
                    return RedirectToAction("TrainerClients", "Client");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(clientId, "Unexpected error occured please try again later!");
            }
            return View("~/Views/Home/Error500.cshtml");
        }

        [HttpGet]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> AddDiet(string id)
        {
            // Get the current trainer's ID
            var trainerId = User.GetId();

            // Call the service to retrieve all diets associated with the trainer and client
            var model = await trainerService.GetAllTrainerDiets(trainerId, id);

            // If no diets are found for the trainer and client, return an empty view
            if (model == null)
            {
                // If no diets are found, return a view without any model data
                return View();
            }

            // If diets are found, return the view with the model data
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> AddDietToClient(int dietId, string clientId)
        {
            try
            {
                // Attempt to add the diet to the client asynchronously
                var addDiet = await trainerService.AddDietToClient(dietId, clientId);

                // If the diet is successfully added, redirect to the "TrainerClients" action in the "Client" controller
                if (addDiet)
                {
                    return RedirectToAction("TrainerClients", "Client");
                }
            }
            catch (Exception)
            {
                // If an exception occurs during the process, add a model error with a message and return an error view
                ModelState.AddModelError(clientId, "An unexpected error occurred. Please try again later!");
            }

            return View("~/Views/Home/Error500.cshtml");
        }


        [HttpGet]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> RemoveProgram(int id, string clientId)
        {
            // Attempt to remove the program from the client asynchronously
            var removeProgram = await trainerService.RemoveProgramFromClientAsync(id, clientId);

            // If the program is successfully removed, redirect to the "TrainerClients" action in the "Client" controller
            if (removeProgram)
            {
                return RedirectToAction("TrainerClients", "Client");
            }

            // If removing the program fails, add a model error with a message and return an error view
            ModelState.AddModelError(clientId, "An unexpected error occurred. Please try again later.");

            return View("~/Views/Home/Error500.cshtml");
        }

        [HttpGet]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> RemoveDiet(int id, string clientId)
        {
            // Attempt to remove the diet from the client asynchronously
            var removeDiet = await trainerService.RemoveDietFromClientAsync(id, clientId);

            // If the diet is successfully removed, redirect to the "TrainerClients" action in the "Client" controller
            if (removeDiet)
            {
                return RedirectToAction("TrainerClients", "Client");
            }

            // If removing the diet fails, add a model error with a message and return an error view
            ModelState.AddModelError(clientId, "An unexpected error occurred. Please try again later.");

            return View("~/Views/Home/Error500.cshtml");
        }
    }
}
