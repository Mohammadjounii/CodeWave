using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeWave.Web.Controllers
{
    [Authorize]
    public class PythonLearningPath : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Tree", "LearningPath", new { learningPath = "Python" });
        }
    }
}
