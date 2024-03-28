using Microsoft.AspNetCore.Mvc;

namespace Pumping_Iron.Core.Controllers
{
    public class TrainingProgramController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
