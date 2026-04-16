using CodeWave.Domain.Entities;
using CodeWave.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace CodeWave.Web.Controllers
{
    public class JobOfferViewModel
    {
        public JobOffer Job { get; set; }
        public double MatchPercentage { get; set; }
        public bool HasApplied { get; set; }
    }

    [Authorize]
    public class JobOfferController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJobOfferService _jobOfferService;
        private readonly IJobApplicationService _jobApplicationService;
        private readonly ICVService _cvService;
        private readonly IUserService _userService;
        private readonly ILearningPathService _learningPathService;

        public JobOfferController(
            UserManager<ApplicationUser> userManager, 
            IJobOfferService jobOfferService,
            IJobApplicationService jobApplicationService,
            ICVService cvService,
            IUserService userService,
            ILearningPathService learningPathService)
        {
            _userManager = userManager;
            _jobOfferService = jobOfferService;
            _jobApplicationService = jobApplicationService;
            _cvService = cvService;
            _userService = userService;
            _learningPathService = learningPathService;
        }

        public async Task<IActionResult> Index(string searchTerm = "", Guid? selectedJobId = null)
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

            // Get user's CV to extract skills
            var cv = await _cvService.GetCVByUserIdAsync(userGuid);
            var userSkills = ExtractSkills(cv?.ProgrammingLanguages ?? string.Empty);

            // Get active job offers
            var jobOffers = await _jobOfferService.GetActiveJobOffersAsync(searchTerm);

            // Calculate match percentage for each job
            var jobsWithMatch = new List<JobOfferViewModel>();
            foreach (var job in jobOffers)
            {
                var requiredSkills = ExtractSkills(job.RequiredSkills ?? string.Empty);
                var matchPercentage = CalculateSkillMatch(userSkills, requiredSkills);
                
                // Check if user has already applied
                var hasApplied = await _jobApplicationService.HasUserAppliedAsync(userGuid, job.Id);

                jobsWithMatch.Add(new JobOfferViewModel
                {
                    Job = job,
                    MatchPercentage = matchPercentage,
                    HasApplied = hasApplied
                });
            }
            
            jobsWithMatch = jobsWithMatch.OrderByDescending(j => j.MatchPercentage).ToList();

            // Get selected job details
            JobOffer selectedJob = null;
            if (selectedJobId.HasValue)
            {
                selectedJob = await _jobOfferService.GetJobOfferByIdAsync(selectedJobId.Value);
            }
            else if (jobsWithMatch.Any())
            {
                selectedJob = jobsWithMatch.First().Job;
            }

            // Get user's existing applications
            var userApplications = await _jobApplicationService.GetUserApplicationIdsAsync(userGuid);

            ViewBag.Jobs = jobsWithMatch;
            ViewBag.SelectedJob = selectedJob;
            ViewBag.SelectedJobId = selectedJob?.Id;
            ViewBag.UserSkills = userSkills;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.UserApplications = userApplications;
            ViewBag.UserGuid = userGuid;
            ViewBag.User = user; // Pass user for navigation
            ViewBag.LearningPath = user.LearningPath; // For dashboard navigation
            
            // Get recommended course based on user's learning path
            var recommendedCourse = await _userService.GetRecommendedCourseByLearningPathAsync(user.LearningPath ?? string.Empty);
            ViewBag.RecommendedCourse = recommendedCourse;
            
            // Get next lesson (latest lesson user is at) for sidebar navigation
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
            ViewBag.NextLesson = nextLesson;
            
            // For sidebar navigation
            var fullName = user.FirstName;
            if (!string.IsNullOrEmpty(user.LastName))
            {
                fullName += " " + user.LastName;
            }
            var level = !string.IsNullOrEmpty(user.Level) ? user.Level + " Developer" : "Developer";
            ViewBag.UserName = fullName;
            ViewBag.UserLevel = level;
            ViewBag.ProfilePictureUrl = user.ProfilePictureUrl;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(Guid jobId, string coverLetter = "")
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                return RedirectToAction("Login", "User");
            }

            // Check if user already applied
            var existingApplication = await _jobApplicationService.GetExistingApplicationAsync(userGuid, jobId);

            if (existingApplication != null)
            {
                TempData["Error"] = "You have already applied to this job.";
                return RedirectToAction("Index", new { selectedJobId = jobId });
            }

            // Get job offer
            var jobOffer = await _jobOfferService.GetJobOfferByIdAsync(jobId);
            if (jobOffer == null)
            {
                TempData["Error"] = "Job offer not found or has been removed.";
                return RedirectToAction("Index");
            }

            // Check if deadline has passed
            if (jobOffer.Deadline < DateTime.UtcNow)
            {
                TempData["Error"] = "Application deadline for this job has passed.";
                return RedirectToAction("Index", new { selectedJobId = jobId });
            }

            // Get user's CV and calculate match
            var cv = await _cvService.GetCVByUserIdAsync(userGuid);
            var userSkills = ExtractSkills(cv?.ProgrammingLanguages ?? string.Empty);
            var requiredSkills = ExtractSkills(jobOffer.RequiredSkills ?? string.Empty);
            var matchPercentage = CalculateSkillMatch(userSkills, requiredSkills);

            // Create application
            var application = new JobApplication
            {
                Id = Guid.NewGuid(),
                UserId = userGuid,
                JobOfferId = jobId,
                AppliedDate = DateTime.UtcNow,
                Status = "Pending",
                CoverLetter = coverLetter ?? string.Empty,
                MatchPercentage = matchPercentage
            };

            await _jobApplicationService.CreateApplicationAsync(application);

            TempData["Success"] = "Application submitted successfully!";
            return RedirectToAction("Index", new { selectedJobId = jobId });
        }

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

            // Get user's job applications with job offer details
            var applications = await _jobApplicationService.GetUserApplicationsAsync(userGuid);

            // For sidebar navigation
            var fullName = user.FirstName;
            if (!string.IsNullOrEmpty(user.LastName))
            {
                fullName += " " + user.LastName;
            }
            var level = !string.IsNullOrEmpty(user.Level) ? user.Level + " Developer" : "Developer";
            
            // Get recommended course based on user's learning path
            var recommendedCourse = await _userService.GetRecommendedCourseByLearningPathAsync(user.LearningPath ?? string.Empty);

            ViewBag.Applications = applications;
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
        public async Task<IActionResult> Withdraw(Guid applicationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                return RedirectToAction("Login", "User");

            var success = await _jobApplicationService.WithdrawApplicationAsync(applicationId, userGuid);
            TempData[success ? "Success" : "Error"] = success
                ? "Application withdrawn successfully."
                : "Could not withdraw this application. Only pending applications can be withdrawn.";

            return RedirectToAction(nameof(AppliedJobs));
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

        // Calculate skill match percentage
        private double CalculateSkillMatch(List<string> userSkills, List<string> requiredSkills)
        {
            if (requiredSkills.Count == 0)
                return 100.0; // If no skills required, consider it a perfect match

            if (userSkills.Count == 0)
                return 0.0; // No user skills means no match

            // Count how many REQUIRED skills are covered by any user skill (exact match).
            // Iterating required skills (not user skills) ensures the result stays in 0–100%.
            var matchedCount = requiredSkills.Count(reqSkill =>
                userSkills.Any(userSkill =>
                    userSkill.Equals(reqSkill, StringComparison.OrdinalIgnoreCase)
                )
            );

            return Math.Round((double)matchedCount / requiredSkills.Count * 100, 1);
        }
    }
}
