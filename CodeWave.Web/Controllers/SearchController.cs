using CodeWave.Application.DTOs;
using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeWave.Web.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SearchController(ISearchService searchService, UserManager<ApplicationUser> userManager)
        {
            _searchService = searchService;
            _userManager   = userManager;
        }

        public async Task<IActionResult> Index(string q)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "User");

            var fullName = user.FirstName + (!string.IsNullOrEmpty(user.LastName) ? " " + user.LastName : "");
            ViewBag.UserName          = fullName;
            ViewBag.UserLevel         = !string.IsNullOrEmpty(user.Level) ? user.Level + " Developer" : "Developer";
            ViewBag.UserLevelRaw      = user.Level;
            ViewBag.LearningPath      = user.LearningPath;
            ViewBag.LearningPathName  = user.LearningPath;
            ViewBag.ProfilePictureUrl = user.ProfilePictureUrl;

            var query = q?.Trim() ?? string.Empty;
            ViewBag.Query = query;

            if (string.IsNullOrWhiteSpace(query))
            {
                SetEmptyViewBag();
                return View();
            }

            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
            var results = await _searchService.SearchAsync(query, userId == Guid.Empty ? null : userId);

            ViewBag.Courses      = results.Where(r => r.Type == "Course").ToList();
            ViewBag.Lessons      = results.Where(r => r.Type == "Lesson").ToList();
            ViewBag.Quizzes      = results.Where(r => r.Type == "Quiz").ToList();
            ViewBag.Achievements = results.Where(r => r.Type == "Achievement").ToList();
            ViewBag.Interviews   = results.Where(r => r.Type == "Interview").ToList();
            ViewBag.Jobs         = results.Where(r => r.Type == "Job").ToList();
            ViewBag.Exercises    = results.Where(r => r.Type == "Exercise").ToList();
            ViewBag.Projects     = results.Where(r => r.Type == "Project").ToList();
            ViewBag.Features     = results.Where(r => r.Type == "Feature").ToList();
            ViewBag.TotalCount   = results.Count;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Results(string q)
        {
            if (string.IsNullOrWhiteSpace(q) || q.Trim().Length < 2)
                return Json(new { query = q ?? "", total = 0, results = Array.Empty<object>() });

            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
            var results = await _searchService.SearchAsync(q.Trim(), userId == Guid.Empty ? null : userId);

            return Json(new
            {
                query   = q.Trim(),
                total   = results.Count,
                results = results.Select(r => new {
                    id              = r.Id,
                    type            = r.Type,
                    title           = r.Title,
                    excerpt         = r.Excerpt,
                    learningPath    = r.LearningPath,
                    difficultyLevel = r.DifficultyLevel,
                    url             = r.Url,
                    typeIcon        = r.TypeIcon,
                    typeColor       = r.TypeColor
                })
            });
        }

        [HttpGet]
        public async Task<IActionResult> Suggestions(string q)
        {
            if (string.IsNullOrWhiteSpace(q) || q.Trim().Length < 2)
                return Json(new List<object>());

            Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
            var results = await _searchService.SuggestAsync(q.Trim(), userId == Guid.Empty ? null : userId);

            return Json(results.Select(r => new
            {
                r.Title,
                r.Type,
                r.TypeIcon,
                r.TypeColor,
                r.Url,
                r.LearningPath,
                r.Excerpt
            }));
        }

        private void SetEmptyViewBag()
        {
            var empty = new List<SearchResultDto>();
            ViewBag.Courses = ViewBag.Lessons = ViewBag.Quizzes =
            ViewBag.Achievements = ViewBag.Interviews = ViewBag.Jobs =
            ViewBag.Exercises = ViewBag.Projects = ViewBag.Features = empty;
            ViewBag.TotalCount = 0;
        }
    }
}
