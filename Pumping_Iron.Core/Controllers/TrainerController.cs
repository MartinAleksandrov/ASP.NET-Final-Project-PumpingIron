namespace Pumping_Iron.Core.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Pumping_Iron.Infrastructure.Extensions;
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
                return BadRequest();
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
                ModelState.AddModelError(clientId,"Unexpected error occured please try again later!");
            }
            return BadRequest();
        }
    }
}
