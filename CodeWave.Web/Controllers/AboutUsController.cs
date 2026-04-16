using Microsoft.AspNetCore.Mvc;

namespace CodeWave.Web.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
