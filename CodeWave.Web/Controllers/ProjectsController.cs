using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeWave.Web.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserService _userService;

        public ProjectsController(
            UserManager<ApplicationUser> userManager,
            IProjectRepository projectRepository,
            IUserService userService)
        {
            _userManager = userManager;
            _projectRepository = projectRepository;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var (user, userGuid) = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "User");

            var projects = await _projectRepository.GetUserProjectsAsync(userGuid);
            await SetSidebarViewBag(user);
            return View(projects);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var (user, _) = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "User");

            await SetSidebarViewBag(user);
            return View("CreateEdit", new Project());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var (user, userGuid) = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "User");

            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null || project.UserId != userGuid)
                return NotFound();

            await SetSidebarViewBag(user);
            return View("CreateEdit", project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Project model)
        {
            var (user, userGuid) = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "User");

            if (!ModelState.IsValid)
            {
                await SetSidebarViewBag(user);
                return View("CreateEdit", model);
            }

            if (model.Id == Guid.Empty)
            {
                await _projectRepository.AddAsync(new Project
                {
                    Id = Guid.NewGuid(),
                    UserId = userGuid,
                    Title = model.Title,
                    Description = model.Description,
                    CompletionDate = model.CompletionDate,
                    Result = model.Result,
                    CreatedAt = DateTime.UtcNow
                });
                TempData["Success"] = "Project added successfully!";
            }
            else
            {
                var existing = await _projectRepository.GetByIdAsync(model.Id);
                if (existing == null || existing.UserId != userGuid)
                    return NotFound();

                existing.Title = model.Title;
                existing.Description = model.Description;
                existing.CompletionDate = model.CompletionDate;
                existing.Result = model.Result;
                await _projectRepository.UpdateAsync(existing);
                TempData["Success"] = "Project updated successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var (user, userGuid) = await GetCurrentUserAsync();
            if (user == null) return RedirectToAction("Login", "User");

            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null || project.UserId != userGuid)
                return NotFound();

            await _projectRepository.DeleteAsync(id);
            TempData["Success"] = "Project deleted.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<(ApplicationUser? user, Guid userGuid)> GetCurrentUserAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                return (null, Guid.Empty);

            var user = await _userManager.FindByIdAsync(userId);
            return (user, userGuid);
        }

        private async Task SetSidebarViewBag(ApplicationUser user)
        {
            var fullName = (user.FirstName ?? "").Trim();
            if (!string.IsNullOrEmpty(user.LastName))
                fullName += " " + user.LastName;

            ViewBag.UserName = fullName;
            ViewBag.UserLevel = !string.IsNullOrEmpty(user.Level) ? user.Level + " Developer" : "Developer";
            ViewBag.UserLevelRaw = user.Level;
            ViewBag.ProfilePictureUrl = user.ProfilePictureUrl;
            ViewBag.LearningPath = user.LearningPath;
            ViewBag.RecommendedCourse = await _userService.GetRecommendedCourseByLearningPathAsync(user.LearningPath ?? string.Empty);
        }
    }
}
