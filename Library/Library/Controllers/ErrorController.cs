using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult FailedRegister()
        {
            return View();
        }

        public IActionResult FailedLogin()
        {
            return View();
        }
    }
}