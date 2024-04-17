namespace Pumping_Iron.Core.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Pumping_Iron.Data.ViewModels.Client;
    using Pumping_Iron.Infrastructure.Extensions;
    using Pumping_Iron.Services.Interfaces;

    public class ClientController : Controller
    {
        private readonly IClientService clientService;
        private readonly ITrainerService trainerService;


        public ClientController(IClientService service, ITrainerService trainerService)
        {
            clientService = service;
            this.trainerService = trainerService;
        }

        [HttpGet]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> TrainerClients()
        {
            var userId = User.GetId();

            // Retrieve clients associated with the current trainer
            var clients = await clientService.GetMyClientsAsync(userId);

            // Check if clients were successfully retrieved
            if (clients == null)
            {
                return BadRequest("Trainer does not exist or error occurred.");
            }

            // If clients were successfully retrieved, return a View with the clients data
            return View(clients);
        }

        [HttpGet]
        public IActionResult Hire(string trainerId)
        {
            var model = new HireTrainerViewModel()
            {
                TrainerId = Guid.Parse(trainerId)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Hire(HireTrainerViewModel model)
        {
            var clientId = User.GetId();
            var trainerId = model.TrainerId.ToString(); 

            var isHired = await trainerService.HireTrainerAsync(model, trainerId,clientId);

            if (isHired)
            {
                return RedirectToAction("AllTrainers", "Trainer");
            }

            return BadRequest("Something goes wrong, please try again later");

        }

        [HttpGet]
        public async Task<IActionResult> MyTrainer()
        {
            var userId = User.GetId();

            var model = await clientService.GetMyTrainerInfo(userId);

            if (model == null)
            {
                return View();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyDiet()
        {
            var userId = User.GetId();

            var model = await clientService.GetMyDietInfo(userId);

            if (model == null)
            {
                return View();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyProgram()
        {
            var userId = User.GetId();

            var model = await clientService.GetMyProgramInfo(userId);

            if (model == null)
            {
                return View();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveTrainer()
        {
            var userId = User.GetId();

            try
            {
                var result = await clientService.RemoveMyTrainer(userId);
            }
            catch (Exception)
            {

                ModelState.AddModelError(userId, "Unexpected error occured please try again later!");
            }

            return RedirectToAction("AllTrainers","Trainer");
        }
    }
}