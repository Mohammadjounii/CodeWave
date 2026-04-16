using CodeWave.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodeWave.Infrastructure.Data;
using System.Security.Claims;

namespace CodeWave.Web.Controllers
{
    [Authorize]
    public class WelcomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        
        public WelcomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            // Fetch courses for Python and Java learning paths
            var courses = await _context.Courses
                .Where(c => !c.IsDeleted && 
                    (c.ProgrammingLanguage == Language.Python || c.ProgrammingLanguage == Language.Java))
                .Include(c => c.Lessons.Where(l => !l.isDeleted))
                .OrderBy(c => c.ProgrammingLanguage)
                .ThenBy(c => c.CreatedAt)
                .ToListAsync();

            // Group courses by programming language and take the first course for each
            var pythonCourse = courses.FirstOrDefault(c => c.ProgrammingLanguage == Language.Python);
            var javaCourse = courses.FirstOrDefault(c => c.ProgrammingLanguage == Language.Java);

            ViewBag.PythonCourse = pythonCourse;
            ViewBag.JavaCourse = javaCourse;

            return View();
        }

        public IActionResult UserInterest()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserInterest(List<string> interests)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userIdGuid))
            {
                return RedirectToAction("Login", "User");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // Save interests as comma-separated string or JSON
                user.Interests = interests != null && interests.Count > 0 
                    ? string.Join(",", interests) 
                    : null;
                
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    TempData["Error"] = "Failed to save interests.";
                }
            }

            return RedirectToAction("UserGoal");
        }

        public IActionResult UserGoal()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserGoal(string goal)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userIdGuid))
            {
                return RedirectToAction("Login", "User");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.Goal = goal;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    TempData["Error"] = "Failed to save goal.";
                }
            }

            return RedirectToAction("UserSkill");
        }

        public IActionResult UserSkill()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserSkill(string skillLevel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userIdGuid))
            {
                return RedirectToAction("Login", "User");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.SkillLevel = skillLevel;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    TempData["Error"] = "Failed to save skill level.";
                }
            }

            return RedirectToAction("UserMotivation");
        }

        public IActionResult UserMotivation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserMotivation(string motivation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userIdGuid))
            {
                return RedirectToAction("Login", "User");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.Motivation = motivation;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    TempData["Error"] = "Failed to save motivation.";
                }
            }

            return RedirectToAction("UserSchedule");
        }

        public IActionResult UserSchedule()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserSchedule(int weeklyHours)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userIdGuid))
            {
                return RedirectToAction("Login", "User");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.WeeklyHours = weeklyHours;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    TempData["Error"] = "Failed to save weekly hours.";
                }
            }

            // After completing onboarding, redirect to language interest selection
            return RedirectToAction("LanguageInterest");
        }

        public IActionResult LanguageInterest()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LanguageInterest(string preferredLanguage)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userIdGuid))
            {
                return RedirectToAction("Login", "User");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.PreferredLanguage = preferredLanguage;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    TempData["Error"] = "Failed to save language preference.";
                }
            }

            // After selecting language preference, redirect to assessment
            return RedirectToAction("Start", "Assessment");
        }
    }
}
