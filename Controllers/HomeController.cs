using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using discgolf.Models;

namespace discgolf.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        public IActionResult Index()
        {
            var jsonStr = System.IO.File.ReadAllText("dg-courses.json");
            var JsonObj = JsonConvert.DeserializeObject<IEnumerable<DgCourses>>(jsonStr);
            return View(JsonObj);
        }

        [Route("/om-sidan")]
        public IActionResult About()
        {
            return View();
        }

    }
}
