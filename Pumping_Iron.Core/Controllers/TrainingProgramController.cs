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


        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            var model = await programService.GetProgramDetailsAsync(Id);

            if (model == null)
            {
                return BadRequest();
            }
           
            return View(model);
        }
    }
}