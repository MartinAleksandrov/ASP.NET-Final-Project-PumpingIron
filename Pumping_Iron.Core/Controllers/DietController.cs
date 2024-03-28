using Microsoft.AspNetCore.Mvc;

namespace Pumping_Iron.Core.Controllers
{
    public class DietController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
