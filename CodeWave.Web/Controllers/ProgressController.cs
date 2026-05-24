using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeWave.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/progress")]
    public class ProgressController : ControllerBase
    {
        private readonly ILessonCompletionRepository _lessonCompletionRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IUserCourseRepository _userCourseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProgressController(
            ILessonCompletionRepository lessonCompletionRepository,
            ILessonRepository lessonRepository,
            IUserCourseRepository userCourseRepository,
            IUnitOfWork unitOfWork)
        {
            _lessonCompletionRepository = lessonCompletionRepository;
            _lessonRepository = lessonRepository;
            _userCourseRepository = userCourseRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("completeLesson")]
        public async Task<IActionResult> CompleteLesson([FromBody] CompleteLessonRequest request)
        {
            if (request == null || request.LessonId == Guid.Empty)
            {
                return BadRequest(new { success = false, message = "Valid lessonId is required." });
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized(new { success = false, message = "User not logged in." });
            }

            var lesson = await _lessonRepository.GetByIdAsync(request.LessonId);
            if (lesson == null)
            {
                return NotFound(new { success = false, message = "Lesson not found." });
            }

            var existing = await _lessonCompletionRepository.GetAsync(userId, request.LessonId);

            if (existing == null)
            {
                var completion = new LessonCompletion
                {
                    Id = Guid.NewGuid(),
                    LessonId = request.LessonId,
                    UserId = userId,
                    IsCompleted = true,
                    CompletionDate = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    isDeleted = false
                };

                await _lessonCompletionRepository.AddAsync(completion);
            }
            else
            {
                existing.IsCompleted = true;
                existing.CompletionDate = DateTime.UtcNow;
            }

            await _unitOfWork.SaveChangesAsync();

            // Check if all lessons in the course are now completed → mark course done
            var allCourseLessons = await _lessonRepository.GetByCourseAsync(lesson.CourseId);
            var completedIds = await _lessonCompletionRepository.GetCompletedLessonIdsAsync(userId);
            var completedSet = completedIds.ToHashSet();
            bool allDone = allCourseLessons.All(l => completedSet.Contains(l.Id));

            if (allDone)
            {
                await _userCourseRepository.MarkCourseCompletedAsync(userId, lesson.CourseId);
            }

            return Ok(new
            {
                success = true,
                message = "Lesson marked as completed.",
                courseCompleted = allDone
            });
        }
    }

    public class CompleteLessonRequest
    {
        public Guid LessonId { get; set; }
    }
}