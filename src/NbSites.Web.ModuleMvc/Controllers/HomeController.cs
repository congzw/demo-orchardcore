using Microsoft.AspNetCore.Mvc;

namespace NbSites.Web.ModuleMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
