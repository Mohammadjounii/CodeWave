using CodeWave.Web.Models;
using CodeWave.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using CodeWave.Application.Interfaces;
using CodeWave.Infrastructure.Data;

namespace CodeWave.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly ILearningPathService _learningPathService;
        private readonly CodeWave.Application.Interfaces.IProgressService _progressService;
        private readonly CodeWave.Application.Interfaces.IQuizService _quizService;
        private readonly CodeWave.Infrastructure.Data.ApplicationDbContext _context;
        private readonly IUserReportService _reportService;
        private readonly IExternalJobService _externalJobService;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            ILearningPathService learningPathService,
            CodeWave.Application.Interfaces.IProgressService progressService,
            CodeWave.Application.Interfaces.IQuizService quizService,
            CodeWave.Infrastructure.Data.ApplicationDbContext context,
            IUserReportService reportService,
            IExternalJobService externalJobService)
        {
            _userManager = userManager;
            _userService = userService;
            _reportService = reportService;
            _learningPathService = learningPathService;
            _progressService = progressService;
            _quizService = quizService;
            _context = context;
            _externalJobService = externalJobService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                return View();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }

            if (!user.OnboardingCompleted && !user.IsAdmin)
            {
                return RedirectToAction("UserInterest", "Welcome");
            }

            // Build full name
            var fullName = user.FirstName;
            if (!string.IsNullOrEmpty(user.LastName))
            {
                fullName += " " + user.LastName;
            }

            // Get level or default to "Developer"
            var level = !string.IsNullOrEmpty(user.Level) ? user.Level + " Developer" : "Developer";

            ViewBag.UserName = fullName;
            ViewBag.UserFirstName = user.FirstName;
            ViewBag.UserLevel = level;
            ViewBag.LearningPath = user.LearningPath; // For Dashboard link redirection
            ViewBag.UserLevelRaw = user.Level; // Raw level for checking if Beginner
            ViewBag.ProfilePictureUrl = user.ProfilePictureUrl; // Profile picture URL
            ViewBag.UserGoal = user.Goal;
            ViewBag.UserMotivation = user.Motivation;
            ViewBag.WeeklyHours = user.WeeklyHours;

            // Get personalized dashboard data based on learning path
            if (!string.IsNullOrEmpty(user.LearningPath))
            {
                // Get user's course progress
                var userCourses = await _userService.GetUserCoursesAsync(userGuid);
                var completedCourses = await _userService.GetCompletedCoursesCountAsync(userGuid);
                var totalProgress = await _userService.GetAverageProgressPercentAsync(userGuid);

                // Get next recommended lesson based on learning path
                var recommendedCourse = await _userService.GetRecommendedCourseByLearningPathAsync(user.LearningPath);
                Lesson nextLesson = null;

                if (recommendedCourse != null)
                {
                    var courseViewModel = await _learningPathService.GetCourseAsync(recommendedCourse.Id, userGuid);
                    if (courseViewModel != null)
                    {
                        // Find first incomplete lesson
                        nextLesson = courseViewModel.Lessons
                            .FirstOrDefault(l => !courseViewModel.CompletedLessonIds.Contains(l.Id));
                        
                        // If all lessons completed, get the last lesson
                        if (nextLesson == null && courseViewModel.Lessons.Any())
                        {
                            nextLesson = courseViewModel.Lessons.Last();
                        }
                    }
                }

                ViewBag.CompletedCourses = completedCourses;
                ViewBag.TotalProgress = totalProgress;
                ViewBag.RecommendedCourse = recommendedCourse;
                ViewBag.NextLesson = nextLesson;
                ViewBag.LearningPathName = user.LearningPath;

                // Get detailed progress data
                var userProgress = await _progressService.GetUserProgressAsync(userGuid);
                var userSkills = await _progressService.GetUserSkillsAsync(userGuid, user.LearningPath);
                var userWeaknesses = await _progressService.GetUserWeaknessesAsync(userGuid, user.LearningPath);
                var totalStudyTime = await _progressService.GetTotalStudyTimeMinutesAsync(userGuid, user.LearningPath);
                var studyTimeByTopic = await _progressService.GetStudyTimeByTopicAsync(userGuid, user.LearningPath);

                ViewBag.UserProgress = userProgress;
                ViewBag.UserSkills = userSkills;
                ViewBag.UserWeaknesses = userWeaknesses;
                ViewBag.TotalStudyTimeMinutes = totalStudyTime;
                ViewBag.StudyTimeByTopic = studyTimeByTopic;

                // Get quiz stats
                var quizAttempts = await _quizService.GetUserQuizAttemptsAsync(userGuid);
                var lpForQuiz = (user.LearningPath ?? "").ToLower();
                var quizLangEnum = lpForQuiz.Contains("python") ? (Language?)Language.Python
                                 : lpForQuiz.Contains("web")    ? (Language?)Language.WebDevelopment
                                 : (Language?)Language.Java;
                var availableQuizCount = await _context.Quizzes
                    .Where(q => !q.IsDeleted && q.Course.ProgrammingLanguage == quizLangEnum)
                    .CountAsync();
                var quizStats = new
                {
                    TotalAttempts = quizAttempts.Count,
                    PassedAttempts = quizAttempts.Count(a => a.IsPassed == true),
                    AvailableQuizzes = availableQuizCount,
                    AverageScore = quizAttempts.Where(a => a.Score.HasValue).Any()
                        ? quizAttempts.Where(a => a.Score.HasValue).Average(a => a.Score.Value)
                        : 0,
                    BestScore = quizAttempts.Where(a => a.Score.HasValue).Any()
                        ? quizAttempts.Where(a => a.Score.HasValue).Max(a => a.Score.Value)
                        : 0
                };
                ViewBag.QuizStats = quizStats;

                // Tree: all courses + lessons + exercises for this learning path
                // Match using Contains (case-insensitive) to align with UserService convention
                var lpLower = user.LearningPath.ToLower().Trim();
                var lpKey   = lpLower.Contains("python") ? "python" :
                              lpLower.Contains("java")   ? "java"   :
                              lpLower.Contains("web")    ? "web"    : lpLower;

                var treeCourses = await _context.Courses
                    .Where(c => !c.IsDeleted && EF.Functions.Like(c.LearningPath, $"%{lpKey}%"))
                    .Include(c => c.Lessons.Where(l => !l.isDeleted).OrderBy(l => l.OrderNumber))
                        .ThenInclude(l => l.CodingExercises.Where(e => !e.isDeleted))
                    .OrderBy(c => c.CreatedAt)
                    .ToListAsync();

                var completedIds = await _context.LessonCompletions
                    .Where(lc => lc.UserId == userGuid && lc.IsCompleted && !lc.isDeleted)
                    .Select(lc => lc.LessonId)
                    .ToListAsync();

                ViewBag.TreeCourses    = treeCourses;
                ViewBag.TreeCompleted  = new HashSet<Guid>(completedIds);
            }

            // Fetch real job recommendations for the dashboard (top 3)
            try
            {
                var jobQuery = (user.LearningPath?.ToLower()) switch
                {
                    var lp when lp != null && lp.Contains("python") => "Python developer",
                    var lp when lp != null && lp.Contains("java") && !lp.Contains("script") => "Java developer",
                    var lp when lp != null && lp.Contains("web") => "Frontend web developer",
                    _ => "software developer"
                };
                var recommendedJobs = await _externalJobService.SearchJobsAsync(jobQuery);
                ViewBag.RecommendedJobs = recommendedJobs.Take(3).ToList();
            }
            catch
            {
                ViewBag.RecommendedJobs = new List<CodeWave.Application.DTOs.ExternalJobDto>();
            }

            return View();
        }

        public async Task<IActionResult> LearningPaths()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                // If user is not authenticated, redirect to home
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            // Redirect based on user's learning path from assessment
            var learningPath = user.LearningPath?.Trim();
            
            if (string.IsNullOrEmpty(learningPath))
            {
                // Default to Python if no learning path is set
                return RedirectToAction("Index", "PythonLearningPath");
            }

            // Map learning path to controller
            return learningPath.ToLower() switch
            {
                "java" => RedirectToAction("Index", "JavaLearningPath"),
                "python" => RedirectToAction("Index", "PythonLearningPath"),
                "web development" or "webdevelopment" => RedirectToAction("Index", "WebDevelopmentLearningPath"),
                _ => RedirectToAction("Index", "PythonLearningPath") // Default fallback
            };
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            if (profilePicture != null && profilePicture.Length > 0)
            {
                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(profilePicture.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(fileExtension))
                {
                    TempData["Error"] = "Invalid file type. Please upload a JPG, PNG, or GIF image.";
                    return RedirectToAction("Index");
                }

                // Validate file size (max 5MB)
                if (profilePicture.Length > 5 * 1024 * 1024)
                {
                    TempData["Error"] = "File size too large. Please upload an image smaller than 5MB.";
                    return RedirectToAction("Index");
                }

                // Create uploads directory if it doesn't exist
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profile-pictures");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate unique filename
                var fileName = $"{userGuid}_{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(stream);
                }

                // Delete old profile picture if exists
                if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfilePictureUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Update user's profile picture URL
                user.ProfilePictureUrl = $"/uploads/profile-pictures/{fileName}";
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["Success"] = "Profile picture updated successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to update profile picture.";
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReport(string category, string subject, string description)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                return RedirectToAction("Login", "User");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return RedirectToAction("Login", "User");

            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(description))
            {
                TempData["Error"] = "Subject and description are required.";
                return RedirectToAction("Index");
            }

            var report = new UserReport
            {
                UserId = userGuid,
                UserName = $"{user.FirstName} {user.LastName}".Trim(),
                UserEmail = user.Email ?? string.Empty,
                Category = category ?? "Other",
                Subject = subject.Trim(),
                Description = description.Trim()
            };

            await _reportService.SubmitReportAsync(report);
            TempData["Success"] = "Your report has been submitted. Thank you for your feedback!";
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
