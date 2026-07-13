using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeWave.Web.Controllers;

[Authorize]
public class AchievementsController : Controller
{
    private readonly IAchievementService _achievements;
    private readonly UserManager<ApplicationUser> _userManager;

    public AchievementsController(IAchievementService achievements, UserManager<ApplicationUser> userManager)
    {
        _achievements = achievements;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            return RedirectToAction("Login", "User");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return RedirectToAction("Login", "User");

        var fullName = user.FirstName + (!string.IsNullOrEmpty(user.LastName) ? " " + user.LastName : "");
        ViewBag.UserName         = fullName;
        ViewBag.UserLevel        = !string.IsNullOrEmpty(user.Level) ? user.Level + " Developer" : "Developer";
        ViewBag.UserLevelRaw     = user.Level;
        ViewBag.LearningPath     = user.LearningPath;
        ViewBag.ProfilePictureUrl = user.ProfilePictureUrl;

        var all      = await _achievements.GetAllAchievementsAsync();
        var earned   = await _achievements.GetUserAchievementsAsync(userGuid);
        var earnedIds = earned.Select(ua => ua.AchievementId).ToHashSet();
        var totalXP  = earned.Sum(ua => ua.Achievement.XPValue);

        ViewBag.All       = all;
        ViewBag.Earned    = earned;
        ViewBag.EarnedIds = earnedIds;
        ViewBag.TotalXP   = totalXP;
        ViewBag.Count     = earned.Count;
        ViewBag.Total     = all.Count;

        return View();
    }
}
