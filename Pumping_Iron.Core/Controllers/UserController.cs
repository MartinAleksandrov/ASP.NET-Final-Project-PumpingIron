namespace Pumping_Iron.Core.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    public class UserController : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
