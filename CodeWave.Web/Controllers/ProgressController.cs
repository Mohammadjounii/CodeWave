using CodeWave.Domain.Entities;
using CodeWave.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeWave.Web.Controllers
{
    [Authorize]
    [Route("api/progress")]   // Base route for all progress operations
    public class ProgressController : Controller
    {
        private readonly ILessonCompletionRepository _lessonCompletionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProgressController(ILessonCompletionRepository lessonCompletionRepository, IUnitOfWork unitOfWork)
        {
            _lessonCompletionRepository = lessonCompletionRepository;
            _unitOfWork = unitOfWork;
        }

        // POST: /api/progress/completeLesson?lessonId=xxxx
        [HttpPost("completeLesson")]
        public async Task<IActionResult> CompleteLesson(Guid lessonId)
        {
            // Get logged-in user's ID (Identity GUID)
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return Unauthorized(new { success = false, message = "User not logged in." });
            }

            // Check if completion already exists
            var existing = await _lessonCompletionRepository.GetAsync(userId, lessonId);

            if (existing == null)
            {
                var completion = new LessonCompletion
                {
                    Id = Guid.NewGuid(),
                    LessonId = lessonId,
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
                // Mark as completed if exists
                existing.IsCompleted = true;
                existing.CompletionDate = DateTime.UtcNow;
            }

            await _unitOfWork.SaveChangesAsync();

            return Ok(new { success = true });
        }
    }
}

