using Microsoft.AspNetCore.Mvc;

namespace Pumping_Iron.Core.Controllers
{
    public class CoachController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
