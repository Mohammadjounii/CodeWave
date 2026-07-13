using CodeWave.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeWave.Web.Controllers
{
    [Authorize]
    public class PlaygroundController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PlaygroundController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "User");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return RedirectToAction("Login", "User");

            var fullName = user.FirstName;
            if (!string.IsNullOrEmpty(user.LastName))
                fullName += " " + user.LastName;

            ViewBag.UserName        = fullName;
            ViewBag.UserLevel       = !string.IsNullOrEmpty(user.Level) ? user.Level + " Developer" : "Developer";
            ViewBag.UserLevelRaw    = user.Level;
            ViewBag.LearningPath    = user.LearningPath;
            ViewBag.ProfilePictureUrl = user.ProfilePictureUrl;

            return View();
        }
    }
}
