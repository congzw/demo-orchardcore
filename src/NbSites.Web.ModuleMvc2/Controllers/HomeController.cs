using Microsoft.AspNetCore.Mvc;

namespace NbSites.Web.ModuleMvc2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
