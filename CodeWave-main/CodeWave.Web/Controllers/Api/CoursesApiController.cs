using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CodeWave.Infrastructure.Data;
using System.Security.Claims;

namespace CodeWave.Web.Controllers.Api;

/// <summary>
/// API Controller for Course operations
/// Exposes endpoints for external parties to interact with CodeWave courses
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class CoursesApiController : ControllerBase
{
    private readonly ICourseRepository _courseRepository;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CoursesApiController> _logger;

    public CoursesApiController(
        ICourseRepository courseRepository,
        ApplicationDbContext context,
        ILogger<CoursesApiController> logger)
    {
        _courseRepository = courseRepository;
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all available courses
    /// </summary>
    /// <returns>List of courses</returns>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
    {
        try
        {
            var courses = await _context.Courses
                .Where(c => !c.IsDeleted)
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    Difficulty = c.DifficultyLevel != null ? c.DifficultyLevel : "Not Specified",
                    Language = c.ProgrammingLanguage != null ? c.ProgrammingLanguage.ToString() : (c.LearningPath != null ? c.LearningPath : "Not Specified"),
                    LessonCount = c.Lessons.Count,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} courses via API", courses.Count);
            return Ok(courses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving courses");
            return StatusCode(500, new { error = "An error occurred while retrieving courses" });
        }
    }

    /// <summary>
    /// Get a specific course by ID
    /// </summary>
    /// <param name="id">Course ID</param>
    /// <returns>Course details</returns>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<CourseDetailDto>> GetCourse(Guid id)
    {
        try
        {
            var course = await _context.Courses
                .Include(c => c.Lessons)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (course == null)
            {
                return NotFound(new { error = "Course not found" });
            }

            var courseDto = new CourseDetailDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Difficulty = course.DifficultyLevel != null ? course.DifficultyLevel : "Not Specified",
                Language = course.ProgrammingLanguage != null ? course.ProgrammingLanguage.ToString() : (course.LearningPath != null ? course.LearningPath : "Not Specified"),
                Lessons = course.Lessons.Select(l => new LessonDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    OrderNumber = l.OrderNumber
                }).OrderBy(l => l.OrderNumber).ToList(),
                CreatedAt = course.CreatedAt
            };

            return Ok(courseDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving course {CourseId}", id);
            return StatusCode(500, new { error = "An error occurred while retrieving the course" });
        }
    }

    /// <summary>
    /// Get user's enrolled courses (requires authentication)
    /// </summary>
    /// <returns>List of enrolled courses</returns>
    [HttpGet("my-courses")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetMyCourses()
    {
        try
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
            {
                return Unauthorized(new { error = "Invalid user" });
            }

            var enrolledCourses = await _context.UserCourses
                .Where(uc => uc.UserId == userId && !uc.Course.IsDeleted)
                .Select(uc => new CourseDto
                {
                    Id = uc.Course.Id,
                    Title = uc.Course.Title,
                    Description = uc.Course.Description,
                    Difficulty = uc.Course.DifficultyLevel != null ? uc.Course.DifficultyLevel : "Not Specified",
                    Language = uc.Course.ProgrammingLanguage != null ? uc.Course.ProgrammingLanguage.ToString() : (uc.Course.LearningPath != null ? uc.Course.LearningPath : "Not Specified"),
                    LessonCount = uc.Course.Lessons.Count(l => !l.isDeleted),
                    CreatedAt = uc.Course.CreatedAt,
                    EnrolledAt = uc.CreatedAt
                })
                .ToListAsync();

            _logger.LogInformation("User {UserId} retrieved {Count} enrolled courses", userId, enrolledCourses.Count);
            return Ok(enrolledCourses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user courses");
            return StatusCode(500, new { error = "An error occurred while retrieving your courses" });
        }
    }
}

// DTOs for API responses
public class CourseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public int LessonCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? EnrolledAt { get; set; }
}

public class CourseDetailDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public List<LessonDto> Lessons { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class LessonDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int OrderNumber { get; set; }
}
