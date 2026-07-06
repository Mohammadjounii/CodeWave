using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeWave.Web.Controllers
{
    [Authorize]
    public class JavaLearningPathController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Tree", "LearningPath", new { learningPath = "Java" });
        }
    }
}
