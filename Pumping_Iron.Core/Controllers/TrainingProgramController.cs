namespace Pumping_Iron.Core.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    public class TrainingProgramController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
