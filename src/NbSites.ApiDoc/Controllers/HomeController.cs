using Microsoft.AspNetCore.Mvc;

namespace NbSites.ApiDoc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
