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



            if (Request.Cookies["createdCourses"] != null)
            {
                var temp = "[" + Request.Cookies["createdCourses"] + "]";

                var createdCourses = JsonConvert.DeserializeObject<IEnumerable<DgCourses>>(temp);

                ViewBag.CreatedCourses = createdCourses;

            }



            return View(JsonObj);
        }

        [Route("/AddCourse")]
        public IActionResult AddCourse()
        {
            int numberOfBaskets = TempData["NumberOfBaskets"] != null ? Convert.ToInt32(TempData["NumberOfBaskets"]) : 0;

            // Skicka numberOfBaskets till vyn
            ViewBag.NumberOfBaskets = numberOfBaskets;

            return View();
        }

        [HttpPost]
        [Route("/AddCourse")]
        public IActionResult AddCourse(DgCourses model, int numberOfBaskets)
        {
            //Validera
            if (ModelState.IsValid)
            {
                string jsonStr = JsonConvert.SerializeObject(model);
                //TempData["new-course"] = jsonStr;
                //TempData["new-course"] = model;

                // Hämta värde från cookie
                //if (Request.Cookies["createdCourses"] == null){
                //}
                var createdCourses = Request.Cookies["createdCourses"];

                string courses = jsonStr + "," + createdCourses;

                // Sätta värde i cookie
                Response.Cookies.Append("createdCourses", courses, new CookieOptions
                {
                    Expires = DateTime.Now.AddYears(2),
                    HttpOnly = true,
                    Secure = true
                });

                /*  Response.Cookies.Append("createdCourses", "", new CookieOptions
                  {
                      Expires = DateTime.Now.AddDays(-1),
                      HttpOnly = true,
                      Secure = true
                  });*/

                return RedirectToAction("Index");

            }
            else
            {

                ViewBag.NumberOfBaskets = numberOfBaskets;

                ViewData["ErrorMessage"] = "Valideringen misslyckades. Kontrollera dina inmatningar.";
                return View();

            }

        }

        public IActionResult Play()
        {
            /*   var jsonStr = System.IO.File.ReadAllText("dg-courses.json");
                  var JsonObj = JsonConvert.DeserializeObject<IEnumerable<DgCourses>>(jsonStr);



                  if (Request.Cookies["createdCourses"] != null)
                  {
                      var temp = "[" + Request.Cookies["createdCourses"] + "]";

                      var createdCourses = JsonConvert.DeserializeObject<IEnumerable<DgCourses>>(temp);

                      ViewBag.CreatedCourses = createdCourses;

                  }*/

            return View();
        }

        [Route("/om-sidan")]
        public IActionResult About()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(int numberOf)
        {
            TempData["NumberOfBaskets"] = numberOf;
            // Hanterar klick

            int numberOfBaskets = TempData["NumberOfBaskets"] != null ? Convert.ToInt32(TempData["NumberOfBaskets"]) : 0;

            if (numberOfBaskets <= 0)
            {
                TempData["ErrorMessage"] = "Antalet korgar måste vara större än 0.";
                return View();

            }

            return RedirectToAction("AddCourse");
        }

    }
}
