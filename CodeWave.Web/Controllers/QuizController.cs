using CodeWave.Application.DTOs;
using CodeWave.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CodeWave.Domain.Entities;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Web.Controllers
{
    [Authorize]
    public class QuizController : Controller
    {
        private readonly IQuizService _quizService;
        private readonly ILearningPathService _learningPathService;
        private readonly IQuizRepository _quizRepository;
        private readonly ILessonCompletionRepository _lessonCompletionRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CodeWave.Infrastructure.Data.ApplicationDbContext _context; // Keep for UserQuizAnswers query

        public QuizController(
            IQuizService quizService,
            ILearningPathService learningPathService,
            IQuizRepository quizRepository,
            ILessonCompletionRepository lessonCompletionRepository,
            ICourseRepository courseRepository,
            IUserService userService,
            UserManager<ApplicationUser> userManager,
            CodeWave.Infrastructure.Data.ApplicationDbContext context)
        {
            _quizService = quizService;
            _learningPathService = learningPathService;
            _quizRepository = quizRepository;
            _lessonCompletionRepository = lessonCompletionRepository;
            _courseRepository = courseRepository;
            _userService = userService;
            _userManager = userManager;
            _context = context; // Keep for UserQuizAnswers which doesn't have a repository yet
        }

        // GET: /Quiz - List available quizzes based on covered lessons
        public async Task<IActionResult> Index()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            // Get ALL quizzes directly from database - simplest approach
            var allQuizzesFromDb = await _quizRepository.GetAllQuizzesAsync();

            var allQuizzes = allQuizzesFromDb
                .Where(q => q.Questions.Any(qq => !qq.IsDeleted)) // Only quizzes with active questions
                .Select(q => new QuizDto
                {
                    Id = q.Id,
                    CourseId = q.CourseId,
                    Title = q.Title,
                    Description = q.Description,
                    TimeLimitMinutes = q.TimeLimitMinutes,
                    PassingScore = q.PassingScore,
                    QuestionCount = q.Questions.Count(qq => !qq.IsDeleted),
                    Questions = q.Questions
                        .Where(qq => !qq.IsDeleted)
                        .OrderBy(qq => qq.OrderNumber)
                        .Select(qq => new QuizQuestionDto
                        {
                            Id = qq.Id,
                            Text = qq.Text,
                            Difficulty = qq.Difficulty,
                            OrderNumber = qq.OrderNumber,
                            AnswerOptions = qq.AnswerOptions
                                .Where(ao => !ao.IsDeleted)
                                .Select(ao => new QuizAnswerOptionDto
                                {
                                    Id = ao.Id,
                                    Text = ao.Text,
                                    IsCorrect = ao.IsCorrect
                                })
                                .OrderBy(ao => Guid.NewGuid()) // Shuffle options
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();

            // Get completed lesson IDs
            var completedLessonIds = await _lessonCompletionRepository.GetCompletedLessonIdsAsync(userId);

            // Get user's quiz attempts for performance tracking
            var attempts = await _quizService.GetUserQuizAttemptsAsync(userId);
            ViewBag.QuizAttempts = attempts;
            ViewBag.CompletedLessonIds = completedLessonIds;

            // Populate ViewBag with user data for shared sidebar
            var user = await _userManager.FindByIdAsync(userIdStr);
            if (user != null)
            {
                var fullName = user.FirstName;
                if (!string.IsNullOrEmpty(user.LastName))
                {
                    fullName += " " + user.LastName;
                }
                var level = !string.IsNullOrEmpty(user.Level) ? user.Level + " Developer" : "Developer";

                ViewBag.UserName = fullName;
                ViewBag.UserLevel = level;
                ViewBag.UserLevelRaw = user.Level;
                ViewBag.ProfilePictureUrl = user.ProfilePictureUrl;
                ViewBag.LearningPath = user.LearningPath;
                
                // Get recommended course based on user's learning path
                var recommendedCourse = await _userService.GetRecommendedCourseByLearningPathAsync(user.LearningPath ?? string.Empty);
                ViewBag.RecommendedCourse = recommendedCourse;
            }

            return View(allQuizzes);
        }

        // GET: /Quiz/Take/{quizId} - Start/take a quiz
        public async Task<IActionResult> Take(Guid quizId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var quiz = await _quizService.GetQuizByIdAsync(quizId);
            if (quiz == null)
                return NotFound();

            // Start quiz attempt
            var attempt = await _quizService.StartQuizAsync(userId, quizId);

            // Get course title for footer
            var course = await _courseRepository.GetByIdAsync(quiz.CourseId);
            var courseTitle = course?.Title ?? "Course";

            ViewBag.AttemptId = attempt.Id;
            ViewBag.TimeLimitMinutes = quiz.TimeLimitMinutes;
            ViewBag.CourseTitle = courseTitle;

            return View(quiz);
        }

        // POST: /Quiz/Submit - Submit quiz answers
        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] SubmitQuizRequest request)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            try
            {
                var result = await _quizService.SubmitQuizAsync(request.AttemptId, request.Answers);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: /Quiz/Results/{attemptId} - View quiz results
        public async Task<IActionResult> Results(Guid attemptId)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var attempts = await _quizService.GetUserQuizAttemptsAsync(userId);
            var attempt = attempts.FirstOrDefault(a => a.Id == attemptId);

            if (attempt == null)
                return NotFound();

            // Get detailed quiz info
            var quiz = await _quizService.GetQuizByIdAsync(attempt.QuizId);
            if (quiz == null)
                return NotFound();

            // Get user's answers for this attempt
            var userAnswers = await _context.UserQuizAnswers
                .Include(uqa => uqa.QuizQuestion)
                    .ThenInclude(qq => qq.AnswerOptions)
                .Where(uqa => uqa.UserQuizAttemptId == attemptId && !uqa.IsDeleted)
                .ToListAsync();

            var questionResults = new List<QuestionResultDto>();
            foreach (var answer in userAnswers)
            {
                var correctOption = answer.QuizQuestion.AnswerOptions.FirstOrDefault(ao => ao.IsCorrect);
                questionResults.Add(new QuestionResultDto
                {
                    QuestionId = answer.QuizQuestionId,
                    QuestionText = answer.QuizQuestion.Text,
                    IsCorrect = answer.IsCorrect,
                    SelectedAnswerId = answer.SelectedAnswerOptionId,
                    CorrectAnswerId = correctOption?.Id
                });
            }

            var result = new QuizResultDto
            {
                AttemptId = attemptId,
                Score = attempt.Score ?? 0,
                IsPassed = attempt.IsPassed ?? false,
                CorrectAnswers = questionResults.Count(qr => qr.IsCorrect),
                TotalQuestions = quiz.QuestionCount,
                TimeSpentMinutes = attempt.TimeSpentMinutes,
                QuestionResults = questionResults
            };

            ViewBag.Quiz = quiz;
            ViewBag.Attempt = attempt;

            return View(result);
        }

        // GET: /Quiz/Performance - View user's quiz performance
        public async Task<IActionResult> Performance()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
                return Unauthorized();

            var attempts = await _quizService.GetUserQuizAttemptsAsync(userId);
            
            var stats = new
            {
                TotalAttempts = attempts.Count,
                PassedAttempts = attempts.Count(a => a.IsPassed == true),
                AverageScore = attempts.Where(a => a.Score.HasValue).Any() 
                    ? attempts.Where(a => a.Score.HasValue).Average(a => a.Score.Value) 
                    : 0,
                BestScore = attempts.Where(a => a.Score.HasValue).Any()
                    ? attempts.Where(a => a.Score.HasValue).Max(a => a.Score.Value)
                    : 0
            };

            ViewBag.Stats = stats;
            ViewBag.Attempts = attempts;

            return View();
        }
    }

    public class SubmitQuizRequest
    {
        public Guid AttemptId { get; set; }
        public List<QuizAnswerDto> Answers { get; set; } = new();
    }
}

