using Microsoft.AspNetCore.Mvc;

namespace Pumping_Iron.Core.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
