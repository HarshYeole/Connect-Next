using Microsoft.AspNetCore.Mvc;

namespace WebApplication4
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
