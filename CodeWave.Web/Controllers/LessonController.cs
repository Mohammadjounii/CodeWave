using CodeWave.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CodeWave.Web.Controllers
{
    public class LessonController : Controller
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IExerciseRepository _exerciseRepository;

        public LessonController(ILessonRepository lessonRepository, IExerciseRepository exerciseRepository)
        {
            _lessonRepository = lessonRepository;
            _exerciseRepository = exerciseRepository;
        }

        public async Task<IActionResult> Index(Guid courseId)
        {
            var lessons = await _lessonRepository.GetByCourseAsync(courseId);

            return View(lessons);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var lesson = await _lessonRepository.GetByIdAsync(id);

            if (lesson == null) return NotFound();

            return View(lesson);
        }

        public async Task<IActionResult> Exercise(Guid id)
        {
            var exercise = await _exerciseRepository.GetWithLessonAsync(id);

            if (exercise == null) return NotFound();
            return View(exercise);
        }
    }
}