using Microsoft.AspNetCore.Mvc;

namespace CodeWave.Web.Controllers
{
    public class WebDevelopmentLearningPath : Controller
    {
        public IActionResult Index()
        {
            // Web Development course is not yet available
            TempData["Info"] = "The Web Development learning path is coming soon! Explore our Java or Python courses in the meantime.";
            return RedirectToAction("Index", "Home");
        }
    }
}
