using Microsoft.AspNetCore.Mvc;

namespace Pumping_Iron.Core.Controllers
{
    public class TrainerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
