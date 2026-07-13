using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeWave.Web.Controllers;

[Authorize]
public class InterviewController : Controller
{
    private readonly IInterviewService _interview;
    private readonly UserManager<ApplicationUser> _userManager;

    public InterviewController(IInterviewService interview, UserManager<ApplicationUser> userManager)
    {
        _interview   = interview;
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
        ViewBag.UserName          = fullName;
        ViewBag.UserLevel         = !string.IsNullOrEmpty(user.Level) ? user.Level + " Developer" : "Developer";
        ViewBag.UserLevelRaw      = user.Level;
        ViewBag.LearningPath      = user.LearningPath;
        ViewBag.ProfilePictureUrl = user.ProfilePictureUrl;

        ViewBag.Questions = await _interview.GetAllQuestionsAsync();
        ViewBag.Practices = await _interview.GetUserPracticesAsync(userGuid);

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SavePractice([FromBody] SavePracticeRequest req)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var userGuid))
            return Unauthorized();

        var ok = await _interview.SavePracticeAsync(userGuid, req.QuestionId, req.SelfRating, req.Notes);
        return ok ? Ok(new { success = true }) : BadRequest(new { success = false });
    }

    [HttpPost]
    public async Task<IActionResult> RemovePractice([FromBody] RemovePracticeRequest req)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userId, out var userGuid))
            return Unauthorized();

        var ok = await _interview.RemovePracticeAsync(userGuid, req.QuestionId);
        return ok ? Ok(new { success = true }) : BadRequest(new { success = false });
    }

    public record SavePracticeRequest(Guid QuestionId, int SelfRating, string? Notes);
    public record RemovePracticeRequest(Guid QuestionId);
}
