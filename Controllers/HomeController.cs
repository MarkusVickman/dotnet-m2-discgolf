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

        /* public IActionResult AddCourse()
         {
             int numberOfBaskets = TempData["NumberOfBaskets"] != null ? Convert.ToInt32(TempData["NumberOfBaskets"]) : 0;

             // Skicka numberOfBaskets till vyn
             ViewBag.NumberOfBaskets = numberOfBaskets;

             return View();
         }*/

        public IActionResult Play()
        {

            return View();
        }

        [Route("/om-sidan")]
        public IActionResult About()
        {
            return View();
        }

        [Route("/AddCourse")]
        [HttpPost]
        public IActionResult AddCourse(int numberOf)
        {
            ViewData["NumberOfBaskets"] = numberOf;
            // Hanterar klick

            int numberOfBaskets = ViewData["NumberOfBaskets"] != null ? Convert.ToInt32(ViewData["NumberOfBaskets"]) : 0;

            if (numberOfBaskets <= 0)
            {
                TempData["ErrorMessage"] = "Antalet korgar måste vara större än 0.";
                return RedirectToAction("Index");

            }
            // Skicka numberOfBaskets till vyn
            ViewBag.NumberOfBaskets = numberOfBaskets;

            return View();
        }

    }
}
