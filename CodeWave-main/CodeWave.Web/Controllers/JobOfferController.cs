using CodeWave.Domain.Entities;
using CodeWave.Application.Interfaces;
using CodeWave.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeWave.Web.Controllers
{
    [Authorize]
    public class JobOfferController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICVService _cvService;
        private readonly IUserService _userService;
        private readonly ILearningPathService _learningPathService;
        private readonly IExternalJobService _externalJobService;
        private readonly IExternalJobApplicationService _externalJobApplicationService;

        public JobOfferController(
            UserManager<ApplicationUser> userManager,
            ICVService cvService,
            IUserService userService,
            ILearningPathService learningPathService,
            IExternalJobService externalJobService,
            IExternalJobApplicationService externalJobApplicationService)
        {
            _userManager = userManager;
            _cvService = cvService;
            _userService = userService;
            _learningPathService = learningPathService;
            _externalJobService = externalJobService;
            _externalJobApplicationService = externalJobApplicationService;
        }

        public IActionResult Index(string searchTerm = "", Guid? selectedJobId = null)
        {
            return RedirectToAction(nameof(RealJobs));
        }

        public async Task<IActionResult> RealJobs(string searchTerm = "", string selectedJobId = "")
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                return RedirectToAction("Login", "User");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return RedirectToAction("Login", "User");

            if (!string.IsNullOrEmpty(user.Level) && user.Level.Equals("Beginner", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Jobs section is only available for Intermediate and Advanced users.";
                return RedirectToAction("Index", "Home");
            }

            var cv = await _cvService.GetCVByUserIdAsync(userGuid);
            var userSkills = ExtractSkills(cv?.ProgrammingLanguages ?? string.Empty);

            // Build a meaningful search query from the user's learning path + search term
            var query = string.IsNullOrWhiteSpace(searchTerm)
                ? BuildDefaultQuery(user.LearningPath)
                : searchTerm;

            var realJobs = await _externalJobService.SearchJobsAsync(query, userSkills);

            // Find selected job
            ExternalJobDto? selectedJob = null;
            if (!string.IsNullOrEmpty(selectedJobId))
                selectedJob = realJobs.FirstOrDefault(j => j.JobId == selectedJobId);
            if (selectedJob == null && realJobs.Any())
                selectedJob = realJobs.First();

            // Sidebar data
            var fullName = user.FirstName + (!string.IsNullOrEmpty(user.LastName) ? " " + user.LastName : "");
            var level = !string.IsNullOrEmpty(user.Level) ? user.Level + " Developer" : "Developer";
            var recommendedCourse = await _userService.GetRecommendedCourseByLearningPathAsync(user.LearningPath ?? string.Empty);

            Lesson? nextLesson = null;
            if (recommendedCourse != null)
            {
                var courseViewModel = await _learningPathService.GetCourseAsync(recommendedCourse.Id, userGuid);
                if (courseViewModel != null)
                {
                    nextLesson = courseViewModel.Lessons.FirstOrDefault(l => !courseViewModel.CompletedLessonIds.Contains(l.Id))
                                 ?? courseViewModel.Lessons.LastOrDefault();
                }
            }

            // Track which real jobs the user already self-reported applying to
            var externalApps = await _externalJobApplicationService.GetUserApplicationsAsync(userGuid);
            var appliedExternalJobIds = externalApps.Select(a => a.ExternalJobId).ToHashSet();

            ViewBag.RealJobs = realJobs;
            ViewBag.SelectedRealJob = selectedJob;
            ViewBag.SelectedJobId = selectedJobId;
            ViewBag.UserSkills = userSkills;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.UserGuid = userGuid;
            ViewBag.User = user;
            ViewBag.LearningPath = user.LearningPath;
            ViewBag.UserName = fullName;
            ViewBag.UserLevel = level;
            ViewBag.ProfilePictureUrl = user.ProfilePictureUrl;
            ViewBag.RecommendedCourse = recommendedCourse;
            ViewBag.NextLesson = nextLesson;
            ViewBag.AppliedExternalJobIds = appliedExternalJobIds;

            return View();
        }

        private static string BuildDefaultQuery(string? learningPath) => learningPath?.ToLower() switch
        {
            "python" => "Python developer",
            "java" => "Java developer",
            "web" => "Frontend developer React",
            _ => "software developer"
        };

        public async Task<IActionResult> AppliedJobs()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                return RedirectToAction("Login", "User");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }

            // Check if user is Beginner - lock access
            if (!string.IsNullOrEmpty(user.Level) && user.Level.Equals("Beginner", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Jobs section is only available for Intermediate and Advanced users. Complete more assessments to unlock this feature.";
                return RedirectToAction("Index", "Home");
            }

            // Get external (real) job applications
            var externalApplications = await _externalJobApplicationService.GetUserApplicationsAsync(userGuid);

            // For sidebar navigation
            var fullName = user.FirstName;
            if (!string.IsNullOrEmpty(user.LastName))
            {
                fullName += " " + user.LastName;
            }
            var level = !string.IsNullOrEmpty(user.Level) ? user.Level + " Developer" : "Developer";

            // Get recommended course based on user's learning path
            var recommendedCourse = await _userService.GetRecommendedCourseByLearningPathAsync(user.LearningPath ?? string.Empty);

            ViewBag.ExternalApplications = externalApplications;
            ViewBag.UserName = fullName;
            ViewBag.UserLevel = level;
            ViewBag.UserLevelRaw = user.Level; // Raw level for checking if Beginner
            ViewBag.ProfilePictureUrl = user.ProfilePictureUrl;
            ViewBag.LearningPath = user.LearningPath;
            ViewBag.RecommendedCourse = recommendedCourse;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyRealJob(
            string externalJobId, string jobTitle, string employerName,
            string? employerLogo, string? jobLocation, string applyLink, double matchPercentage)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                return Json(new { success = false, message = "Not authenticated" });

            if (await _externalJobApplicationService.HasUserAppliedAsync(userGuid, externalJobId))
                return Json(new { success = false, alreadyApplied = true, message = "Already recorded" });

            var application = new ExternalJobApplication
            {
                Id = Guid.NewGuid(),
                UserId = userGuid,
                ExternalJobId = externalJobId,
                JobTitle = jobTitle,
                EmployerName = employerName,
                EmployerLogo = employerLogo,
                JobLocation = jobLocation,
                ApplyLink = applyLink,
                MatchPercentage = matchPercentage,
                AppliedDate = DateTime.UtcNow
            };

            await _externalJobApplicationService.SaveApplicationAsync(application);

            // Achievement: first job application
            var achSvc = HttpContext.RequestServices.GetService<IAchievementService>();
            if (achSvc != null)
                await achSvc.TryAwardAsync(userGuid, "first_job_app");

            return Json(new { success = true });
        }

        // Helper method to extract skills from comma-separated or newline-separated string
        private List<string> ExtractSkills(string skillsString)
        {
            if (string.IsNullOrWhiteSpace(skillsString))
                return new List<string>();

            var skills = new List<string>();
            var separators = new[] { ',', '\n', '\r', ';', '|' };

            foreach (var skill in skillsString.Split(separators, StringSplitOptions.RemoveEmptyEntries))
            {
                var cleaned = skill.Trim();
                if (!string.IsNullOrEmpty(cleaned) && !skills.Contains(cleaned, StringComparer.OrdinalIgnoreCase))
                {
                    skills.Add(cleaned);
                }
            }

            return skills;
        }
    }
}
