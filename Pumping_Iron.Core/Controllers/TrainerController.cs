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
            var trainerId = User.GetId();

            var model = await trainerService.GetAllTrainerPrograms(trainerId, id);

            if (model == null)
            {
                return View();
            }

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
            var trainerId = User.GetId();

            var model = await trainerService.GetAllTrainerDiets(trainerId, id);

            if (model == null)
            {
                return View();
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> AddDietToClient(int dietId, string clientId)
        {
            try
            {
                var addDiet = await trainerService.AddDietToClient(dietId, clientId);

                if (addDiet)
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
        public async Task<IActionResult> RemoveProgram(int id, string clientId)
        {

            var removeProgram = await trainerService.RemoveProgramFromClientAsync(id, clientId);

            if (removeProgram)
            {
                return RedirectToAction("TrainerClients", "Client");
            }

            ModelState.AddModelError(clientId, "Unexpected error occured please try again later");

            return View("~/Views/Home/Error500.cshtml");
        }

        [HttpGet]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> RemoveDiet(int id, string clientId)
        {
            var removeDiet = await trainerService.RemoveDietFromClientAsync(id, clientId);

            if (removeDiet)
            {
                return RedirectToAction("TrainerClients", "Client");
            }

            ModelState.AddModelError(clientId, "Unexpected error occured please try again later");

            return View("~/Views/Home/Error500.cshtml");
        }
    }
}
