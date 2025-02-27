using Microsoft.AspNetCore.Mvc;

namespace SportsPro._Controllers
{
    public class RegistrationController : Controller
    {
        //Index method and current home page
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

    }
}