using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using discgolf.Models;


namespace discgolf.Controllers
{

    //Enda controllern för webbplatsen
    public class HomeController : Controller
    {

        // GET route till Index som returnerar object array från fil samt lägger till discgolf-banor i ViewBag till vy.
        public IActionResult Index()
        {
            //läser in fil och sparar som en samling av objekt
            var jsonStr = System.IO.File.ReadAllText("dg-courses.json");
            var jsonObj = JsonConvert.DeserializeObject<IEnumerable<DgCourses>>(jsonStr);

            /*Om det finns egenskapade banor lagrade i cookie så läses de in samt sparas i en samling av objekt. Dessa skickas med till vyn som ViewBag*/
            if (Request.Cookies["created"] != null)
            {
                var temp = Request.Cookies["created"];
                var createdCourses = JsonConvert.DeserializeObject<IEnumerable<DgCourses>>(temp!);
                ViewBag.CreatedCourses = createdCourses;
            }

            //parameterpassning med model DgCourses
            return View(jsonObj);
        }

        //Post route som endast tar in hur stort formuläret ska vara / hur många korgar den nya banan ska innehålla.
        [HttpPost]
        public IActionResult Index(int numberOf)
        {
            //Lagrar formulärdata i tempdata
            TempData["NumberOfBaskets"] = numberOf;

            //Converterar till int eller till noll om det inte går
            int numberOfBaskets = TempData["NumberOfBaskets"] != null ? Convert.ToInt32(TempData["NumberOfBaskets"]) : 0;

            //kontrollerar att värdet är större än noll annars returnerar ett fel som tempdata tillbaka till samma vy.
            if (numberOfBaskets <= 0)
            {
                TempData["ErrorMessage"] = "Antalet korgar måste vara större än 0.";
                return View();
            }

            //Om vädet är större än noll skickas det med som tempdata till lägg till bana routen.
            return RedirectToAction("AddCourse");
        }

        // GET route till Profile lägger till ett profilobjekt i ViewBag med till vy.
        public IActionResult Profile()
        {

            //Kontrollerar om profil finns i cookie
            if (Request.Cookies["profile"] != null)
            {
                var temp = Request.Cookies["profile"];

                // Deserialisera JSON-strängen till en lista av DgCourses-objekt
                var profile = JsonConvert.DeserializeObject<ProfileModel>(temp!);
                ViewBag.Profile = profile;
            }

            return View();
        }

        // Post route till Profile lägger till ett profilobjekt i Cookie om det validerar korrekt enligt modellen.
        [HttpPost]
        public IActionResult Profile(ProfileModel model)
        {
            //Validerar formulär enligt model
            if (ModelState.IsValid)
            {

                // Serialisera listan tillbaka till en JSON-sträng
                string profile = JsonConvert.SerializeObject(model);

                // Sätta värde i cookie
                Response.Cookies.Append("profile", profile, new CookieOptions
                {
                    Expires = DateTime.Now.AddYears(2),
                    HttpOnly = true,
                    Secure = true
                });

            }

            //Skickar med ViewData med felmeddelande om validering misslyckas
            else
            {
                ViewData["ErrorMessage"] = "Valideringen misslyckades. Kontrollera dina inmatningar.";
                return View();
            }

            //Returnerar en redirect till profilsidan för att även logiken där ska köras.
            return RedirectToAction("Profile");

        }

        //Get route till spelade rundor
        public IActionResult Rounds()
        {

            //kontrollerar om spelade rundor finns, i så fall skapas en samling av object från json-datat i cookie
            if (Request.Cookies["played"] != null)
            {
                var temp = Request.Cookies["played"];
                var createdCourses = JsonConvert.DeserializeObject<IEnumerable<DgCourses>>(temp!);
                ViewBag.CreatedCourses = createdCourses;
            }

            //Om inte så skickas ett felmeddelande med i TempData.
            else
            {
                TempData["ErrorMessage"] = "Du har inga sparade spelade banor.";
            }
            //returnerar vy
            return View();
        }

        //Get route till lägg till bana som initieras från formulär / post.routen Index med tempdata.
        [Route("/ny-bana")]
        public IActionResult AddCourse()
        {
            //Hämtas från för att välja storlek på nästa formulär
            int numberOfBaskets = TempData["NumberOfBaskets"] != null ? Convert.ToInt32(TempData["NumberOfBaskets"]) : 0;

            // Skicka numberOfBaskets till vyn för att bestämma storlek på formulär
            ViewBag.NumberOfBaskets = numberOfBaskets;

            return View();
        }

        //Post route för formulärdata för att skapa ny bana.
        [HttpPost]
        [Route("/ny-bana")]
        public IActionResult AddCourse(DgCourses model, int numberOfBaskets)
        {
            //Validerar med DgCourses modellen
            if (ModelState.IsValid)
            {
                //Läser in cookie för skapade banor
                var createdCourses = Request.Cookies["created"];

                //Om skapader banor inte är tom
                if (!string.IsNullOrEmpty(createdCourses))
                {
                    //Deserialisera JSON-strängen till en lista av DgCourses-objekt
                    var coursesList = JsonConvert.DeserializeObject<List<DgCourses>>(createdCourses);

                    //lägger till den nya banan
                    coursesList!.Add(model);

                    //Serialisera listan tillbaka till en JSON-sträng
                    string courses = JsonConvert.SerializeObject(coursesList);

                    //Sätta värde i cookie med inställningar
                    Response.Cookies.Append("created", courses, new CookieOptions
                    {
                        Expires = DateTime.Now.AddYears(2),
                        HttpOnly = true,
                        Secure = true
                    });
                }

                //Om skapade banor från cookie är tom skapas det nya objektet och sparas i kakan. 
                else
                {

                    //initierar en ny lista och lägger till banan
                    var coursesList = new List<DgCourses> { model };

                    //Serialisera listan tillbaka till en JSON-sträng
                    string updatedCourses = JsonConvert.SerializeObject(coursesList);

                    //Sätta värde i cookie med options
                    Response.Cookies.Append("created", updatedCourses, new CookieOptions
                    {
                        Expires = DateTime.Now.AddYears(2),
                        HttpOnly = true,
                        Secure = true
                    });
                }

                //Returnerar en redirekt till startsidan
                return RedirectToAction("Index");
            }

            //Om validering misslyckades returas skapa bana vyn igen med ett felmeddelande i viewdata och antalkorgar som formuläret ska innehålla
            else
            {
                ViewBag.NumberOfBaskets = numberOfBaskets;
                ViewData["ErrorMessage"] = "Valideringen misslyckades. Kontrollera dina inmatningar.";
                return View();
            }
        }

        //Get route för att spela/spara en spelad bana
        public IActionResult Play(int id, Boolean created)
        {

            //created avgör om banan ska laddas in från cookie eller ifrån json-fil med hjälp av id (id är index för cookie och id för json-fil..)
            if (created)
            {
                //läser in kaka
                var createdCourses = Request.Cookies["created"];

                //Deserialisera JSON-strängen till en lista av DgCourses-objekt
                var coursesList = JsonConvert.DeserializeObject<List<DgCourses>>(createdCourses!);

                //Skickar med objekt med banan
                ViewBag.Course = coursesList![id];
            }
            else
            {
                //Annars läses en lista in med objekt av banor från fil.
                var jsonStr = System.IO.File.ReadAllText("dg-courses.json");
                var jsonObj = JsonConvert.DeserializeObject<List<DgCourses>>(jsonStr);

                //Ett objekt skapas med aktuell bana som skickas med som viewbag
                DgCourses course = jsonObj!.Find(course => course.Id == id)!;
                ViewBag.Course = course;
            }

            return View();
        }

        //Post route för att spara den spelade banan
        [HttpPost]
        public IActionResult Play(DgCourses model)
        {

            //Validerar enligt DgCoursese modellen
            if (ModelState.IsValid)
            {
                //Läser in kaka med spelade banor
                var temp = Request.Cookies["played"];
                int id = 0;

                //Om cookiedata inte är noll
                if (!string.IsNullOrEmpty(temp))
                {
                    //Deserialisera JSON-strängen till en lista av DgCourses-objekt
                    var coursesList = JsonConvert.DeserializeObject<List<DgCourses>>(temp);

                    // Sätt ID till antalet objekt i listan (använde inte längre skarpt på sidan då funktionen inte blev klar)
                    //index används istället för att identifiera banor
                    id = coursesList!.Count;

                    // Lägg till den nya kursen till listan
                    model.Id = id;
                    coursesList.Add(model);

                    //Serialisera listan tillbaka till en JSON-sträng
                    string courses = JsonConvert.SerializeObject(coursesList);

                    //Sätta värde i cookie med inställnignar
                    Response.Cookies.Append("played", courses, new CookieOptions
                    {
                        Expires = DateTime.Now.AddYears(2),
                        HttpOnly = true,
                        Secure = true
                    });
                }

                // Om kakan med spelade banor är tom
                else
                {
                    //en ny lista och lägg till kursen
                    var coursesList = new List<DgCourses> { model };

                    //Serialisera listan tillbaka till en JSON-sträng
                    string updatedCourses = JsonConvert.SerializeObject(coursesList);

                    //Sätta värde i cookie med options
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

        //get about-route 
        [Route("/om-sidan")]
        public IActionResult About()
        {

            //Läser in profil kakan och skickar med den som viewbag till vyn om den finns.
            if (Request.Cookies["profile"] != null)
            {
                var temp = Request.Cookies["profile"];

                //Deserialisera JSON-strängen till ett DgCourses-objekt
                var profile = JsonConvert.DeserializeObject<ProfileModel>(temp!);

                ViewBag.Profile = profile;
            }
            return View();
        }

        //Routen har ingen egen vy utan är bara logik för att ta bort bana
        public IActionResult RemoveCourse(int id)
        {
            //Läser in skapade banor
            var createdCourses = Request.Cookies["created"];

            // Deserialisera JSON-strängen till en lista av DgCourses-objekt
            var coursesList = JsonConvert.DeserializeObject<List<DgCourses>>(createdCourses!);

            // Ta bort elementet vid det specifika indexet
            coursesList!.RemoveAt(id);

            // Serialisera listan tillbaka till en JSON-sträng
            string updatedCourses = JsonConvert.SerializeObject(coursesList);

            // lagrar nya json-arrayen i cookie
            Response.Cookies.Append("created", updatedCourses);

            //dirigerar tillbaka till startsidan
            return RedirectToAction("Index");
        }

        //Routen har ingen egen vy utan är bara logik för att ta bort spelad bana
        public IActionResult RemovePlayedCourse(int id)
        {
            //Läser in spelade banor
            var createdCourses = Request.Cookies["played"];

            // Deserialisera JSON-strängen till en lista av DgCourses-objekt
            var coursesList = JsonConvert.DeserializeObject<List<DgCourses>>(createdCourses!);

            // Ta bort elementet vid det specifika indexet
            coursesList!.RemoveAt(id);

            // Serialisera listan tillbaka till en JSON-sträng
            string updatedCourses = JsonConvert.SerializeObject(coursesList);

            // lagrar nya json-arrayen i cookie
            Response.Cookies.Append("played", updatedCourses);

            //dirigerar tillbaka till spelade rundor
            return RedirectToAction("Rounds");
        }
    }
}
