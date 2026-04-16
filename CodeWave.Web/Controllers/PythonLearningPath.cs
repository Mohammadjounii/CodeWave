using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeWave.Web.Controllers
{
    [Authorize]
    public class PythonLearningPath : Controller
    {
        public IActionResult Index()
        {
            // Redirect to the actual Python course in the LearningPath system
            var pythonCourseId = new Guid("22222222-2222-2222-2222-222222222222");
            return RedirectToAction("Course", "LearningPath", new { courseId = pythonCourseId });
        }
    }
}
