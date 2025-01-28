using Microsoft.AspNetCore.Mvc;

namespace discgolf.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        public IActionResult Index()
        {
            return View();
        }

        [Route("/om-sidan")]
        public IActionResult About()
        {
            return View();
        }

    }
}
