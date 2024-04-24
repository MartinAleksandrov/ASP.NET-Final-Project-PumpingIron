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
        public async Task<IActionResult> TrainerClients(int? page)
        {

            var userId = User.GetId();
            const int pageSize = 4; // Set the number of clients per page

            // Default to page 1 if no page number is specified
            int pageNumber = page ?? 1;

            // Retrieve clients associated with the current trainer with pagination
            var clients = await clientService.GetMyClientsAsync(userId, pageNumber, pageSize);

            // Check if clients were successfully retrieved
            if (clients == null)
            {
                return View("~/Views/Home/Error500.cshtml");
            }

            // If clients were successfully retrieved, return a View with the clients data
            return View(clients);
        }

        [HttpGet]
        [Authorize(Roles = "Client")]
        public IActionResult Hire(string trainerId)
        {
            var model = new HireTrainerViewModel()
            {
                TrainerId = Guid.Parse(trainerId)
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Hire(HireTrainerViewModel model)
        {
            // Get the current user's ID (client ID)
            var clientId = User.GetId();

            // Convert the TrainerId to a string
            var trainerId = model.TrainerId.ToString();

            // Call the service to hire the trainer asynchronously
            var isHired = await trainerService.HireTrainerAsync(model, trainerId, clientId);

            // If the trainer is successfully hired, redirect to the "AllTrainers" action in the "Trainer" controller
            if (isHired)
            {
                return RedirectToAction("AllTrainers", "Trainer");
            }

            // If hiring the trainer fails, return an error view
            return View("~/Views/Home/Error500.cshtml");
        }

        [HttpGet]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> MyTrainer()
        {
            // Get the current user's ID
            var userId = User.GetId();

            // Call the service to retrieve trainer information associated with the user
            var model = await clientService.GetMyTrainerInfo(userId);

            // If no trainer information is found for the user, return an empty view
            if (model == null)
            {
                // If no trainer is found, return a view indicating no trainer is hired yet
                return View();
            }

            // If trainer information is found, return the view with the model data
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> MyDiet()
        {
            // Get the current user's ID
            var userId = User.GetId();

            // Call the service to retrieve diet information associated with the user
            var model = await clientService.GetMyDietInfo(userId);

            // If no diet information is found for the user, return an empty view
            if (model == null)
            {
                // If no diet information is found, return a view indicating no diet plan is available yet
                return View();
            }

            // If diet information is found, return the view with the model data
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> MyProgram()
        {
            // Get the current user's ID
            var userId = User.GetId();

            // Call the service to retrieve program information associated with the user
            var model = await clientService.GetMyProgramInfo(userId);

            // If no program information is found for the user, return an empty view
            if (model == null)
            {
                // If no program information is found, return a view indicating no program is available yet
                return View();
            }

            // If program information is found, return the view with the model data
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> RemoveTrainer()
        {
            var userId = User.GetId();

            try
            {
                var result = await clientService.RemoveMyTrainer(userId);
            }
            catch (Exception)
            {

                ModelState.AddModelError(userId, "Unexpected error occured please try again latver!");
                return View("~/Views/Home/Error500.cshtml");

            }
            return RedirectToAction("AllTrainers","Trainer");
        }
    }
}