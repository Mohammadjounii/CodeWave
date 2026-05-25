using CodeWave.Web.Models;
using CodeWave.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using CodeWave.Application.Interfaces;

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

        public HomeController(
            UserManager<ApplicationUser> userManager, 
            IUserService userService, 
            ILearningPathService learningPathService,
            CodeWave.Application.Interfaces.IProgressService progressService,
            CodeWave.Application.Interfaces.IQuizService quizService)
        {
            _userManager = userManager;
            _userService = userService;
            _learningPathService = learningPathService;
            _progressService = progressService;
            _quizService = quizService;
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
                var quizStats = new
                {
                    TotalAttempts = quizAttempts.Count,
                    PassedAttempts = quizAttempts.Count(a => a.IsPassed == true),
                    AverageScore = quizAttempts.Where(a => a.Score.HasValue).Any()
                        ? quizAttempts.Where(a => a.Score.HasValue).Average(a => a.Score.Value)
                        : 0,
                    BestScore = quizAttempts.Where(a => a.Score.HasValue).Any()
                        ? quizAttempts.Where(a => a.Score.HasValue).Max(a => a.Score.Value)
                        : 0
                };
                ViewBag.QuizStats = quizStats;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
