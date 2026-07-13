using Microsoft.AspNetCore.Mvc;

namespace CodeWave.Web.Controllers
{
    public class WebDevelopmentLearningPath : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Tree", "LearningPath", new { learningPath = "Web Development" });
        }
    }
}
