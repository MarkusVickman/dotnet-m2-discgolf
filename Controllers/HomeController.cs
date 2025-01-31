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
            var jsonObj = JsonConvert.DeserializeObject<IEnumerable<DgCourses>>(jsonStr);


            if (Request.Cookies["created"] != null)
            {
                var temp = Request.Cookies["created"];


                var createdCourses = JsonConvert.DeserializeObject<IEnumerable<DgCourses>>(temp!);

                ViewBag.CreatedCourses = createdCourses;

            }



            return View(jsonObj);
        }



        public IActionResult Rounds()
        {

            if (Request.Cookies["played"] != null)
            {
                var temp = Request.Cookies["played"];

                var createdCourses = JsonConvert.DeserializeObject<IEnumerable<DgCourses>>(temp!);

                ViewBag.CreatedCourses = createdCourses;

            }
            else
            {
                TempData["ErrorMessage"] = "Du har inga sparade spelade banor.";
            }

            return View();
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

                var createdCourses = Request.Cookies["created"];
                //int id = 0;

                if (!string.IsNullOrEmpty(createdCourses))
                {
                    // Deserialisera JSON-strängen till en lista av DgCourses-objekt
                    var coursesList = JsonConvert.DeserializeObject<List<DgCourses>>(createdCourses);

                    //pausar id då det kan bli dubbletter om någon tas bort
                    // Sätt ID till antalet objekt i listan
                    // id = coursesList!.Count;

                    // Lägg till den nya kursen till listan
                    // model.Id = id;
                    coursesList!.Add(model);

                    // Serialisera listan tillbaka till en JSON-sträng
                    string courses = JsonConvert.SerializeObject(coursesList);

                    // Sätta värde i cookie
                    Response.Cookies.Append("created", courses, new CookieOptions
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

                }
                else
                {

                    // Om createdCourses är null eller tom, initiera en ny lista och lägg till kursen
                    var coursesList = new List<DgCourses> { model };

                    // Serialisera listan tillbaka till en JSON-sträng
                    string updatedCourses = JsonConvert.SerializeObject(coursesList);

                    // Sätta värde i cookie
                    Response.Cookies.Append("created", updatedCourses, new CookieOptions
                    {
                        Expires = DateTime.Now.AddYears(2),
                        HttpOnly = true,
                        Secure = true
                    });



                }
                return RedirectToAction("Index");
            }
            else
            {

                ViewBag.NumberOfBaskets = numberOfBaskets;

                ViewData["ErrorMessage"] = "Valideringen misslyckades. Kontrollera dina inmatningar.";
                return View();

            }


        }

        public IActionResult Play(int id, Boolean created)
        {

            if (created)
            {
                var createdCourses = Request.Cookies["created"];



                // Deserialisera JSON-strängen till en lista av DgCourses-objekt
                var coursesList = JsonConvert.DeserializeObject<List<DgCourses>>(createdCourses!);
                //DgCourses course = coursesList!.Find(course => course.Id == id)!;

                ViewBag.Course = coursesList![id];



                // ViewBag.Course = course;

                //                               var createdCourses = JsonConvert.DeserializeObject<IEnumerable<DgCourses>>(temp!);

                //              ViewBag.CreatedCourses = createdCourses;
            }
            else
            {

                var jsonStr = System.IO.File.ReadAllText("dg-courses.json");
                var jsonObj = JsonConvert.DeserializeObject<List<DgCourses>>(jsonStr);

                DgCourses course = jsonObj!.Find(course => course.Id == id)!;



                ViewBag.Course = course;
            }

            return View();
        }

        [HttpPost]
        public IActionResult Play(DgCourses model)
        {



            //Validera
            if (ModelState.IsValid)
            {


                var temp = Request.Cookies["played"];

                int id = 0;

                if (!string.IsNullOrEmpty(temp))
                {
                    // Deserialisera JSON-strängen till en lista av DgCourses-objekt
                    var coursesList = JsonConvert.DeserializeObject<List<DgCourses>>(temp);

                    // Sätt ID till antalet objekt i listan
                    id = coursesList!.Count;

                    // Lägg till den nya kursen till listan
                    model.Id = id;
                    coursesList.Add(model);

                    // Serialisera listan tillbaka till en JSON-sträng
                    string courses = JsonConvert.SerializeObject(coursesList);

                    // Sätta värde i cookie
                    Response.Cookies.Append("played", courses, new CookieOptions
                    {
                        Expires = DateTime.Now.AddYears(2),
                        HttpOnly = true,
                        Secure = true
                    });

                }
                else
                {

                    // Om createdCourses är null eller tom, initiera en ny lista och lägg till kursen
                    var coursesList = new List<DgCourses> { model };

                    // Serialisera listan tillbaka till en JSON-sträng
                    string updatedCourses = JsonConvert.SerializeObject(coursesList);

                    // Sätta värde i cookie
                    Response.Cookies.Append("played", updatedCourses, new CookieOptions
                    {
                        Expires = DateTime.Now.AddYears(2),
                        HttpOnly = true,
                        Secure = true
                    });

                }
            }
            return RedirectToAction("Rounds");
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

        public IActionResult RemoveCourse(int id)
        {

            var createdCourses = Request.Cookies["created"];

            // Deserialisera JSON-strängen till en lista av DgCourses-objekt
            var coursesList = JsonConvert.DeserializeObject<List<DgCourses>>(createdCourses!);

            // Ta bort elementet vid det specifika indexet
            coursesList!.RemoveAt(id);

            // Serialisera listan tillbaka till en JSON-sträng
            string updatedCourses = JsonConvert.SerializeObject(coursesList);

            // Sätta värde i cookie
            Response.Cookies.Append("created", updatedCourses);



            return RedirectToAction("Index");

        }

        public IActionResult RemovePlayedCourse(int id)
        {
            var createdCourses = Request.Cookies["played"];

            // Deserialisera JSON-strängen till en lista av DgCourses-objekt
            var coursesList = JsonConvert.DeserializeObject<List<DgCourses>>(createdCourses!);

            // Ta bort elementet vid det specifika indexet
            coursesList!.RemoveAt(id);

            // Serialisera listan tillbaka till en JSON-sträng
            string updatedCourses = JsonConvert.SerializeObject(coursesList);

            // Sätta värde i cookie
            Response.Cookies.Append("played", updatedCourses);


            return RedirectToAction("Rounds");


        }

    }
}
