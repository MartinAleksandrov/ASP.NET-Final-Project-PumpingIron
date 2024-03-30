namespace Pumping_Iron.Core.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Pumping_Iron.Services.Interfaces;

    public class TrainingProgramController : Controller
    {
        private readonly ITrainingProgramService programService;

        public TrainingProgramController(ITrainingProgramService service)
        {
            programService = service;
        }

        [HttpGet]
        public async Task<IActionResult> AllTrainingPrograms()
        {
            var allTrainingPrograms = await programService.AllTrainingProgramsAsync();

            return View(allTrainingPrograms);
        }
    }
}