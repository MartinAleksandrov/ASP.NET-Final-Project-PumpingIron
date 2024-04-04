namespace Pumping_Iron.Core.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Pumping_Iron.Services.Interfaces;

    [Authorize(Roles = "Coach")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService trainerService;

        public TrainerController(ITrainerService service)
        {
            trainerService = service;
        }

        [Authorize(Roles = "Coach")]
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
    }
}