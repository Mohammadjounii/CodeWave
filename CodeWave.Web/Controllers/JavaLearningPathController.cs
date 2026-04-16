using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeWave.Web.Controllers
{
    [Authorize]
    public class JavaLearningPathController : Controller
    {
        public IActionResult Index()
        {
            // Redirect to the actual Java course in the LearningPath system
            var javaCourseId = new Guid("11111111-1111-1111-1111-111111111111");
            return RedirectToAction("Course", "LearningPath", new { courseId = javaCourseId });
        }
    }
}
