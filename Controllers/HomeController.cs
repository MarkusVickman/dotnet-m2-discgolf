using Microsoft.AspNetCore.Mvc;

namespace discgolf.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        public ActionResult Index()
        {
            return View();
        }

    }
}
