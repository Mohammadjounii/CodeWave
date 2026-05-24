using CodeWave.Domain.Entities;
using CodeWave.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CodeWave.Application.Interfaces;

namespace CodeWave.Web.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        private readonly ILearningPathService _learningPathService;

        public SettingsController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserService userService,
            ILearningPathService learningPathService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _learningPathService = learningPathService;
        }

        public async Task<IActionResult> Index()
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

            // Set ViewBag properties for sidebar
            var fullName = user.FirstName;
            if (!string.IsNullOrEmpty(user.LastName))
            {
                fullName += " " + user.LastName;
            }
            var level = !string.IsNullOrEmpty(user.Level) ? user.Level + " Developer" : "Developer";
            
            ViewBag.UserName = fullName;
            ViewBag.UserLevel = level;
            ViewBag.UserLevelRaw = user.Level;
            ViewBag.ProfilePictureUrl = user.ProfilePictureUrl;
            ViewBag.LearningPath = user.LearningPath;
            ViewBag.LearningPathName = user.LearningPath;

            // Get next lesson (latest lesson user is at) for sidebar navigation
            Lesson nextLesson = null;
            if (!string.IsNullOrEmpty(user.LearningPath))
            {
                var recommendedCourse = await _userService.GetRecommendedCourseByLearningPathAsync(user.LearningPath);

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
            }
            ViewBag.NextLesson = nextLesson;

            var model = new ChangePasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

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

            // Change password with current password verification
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                // Sign out and sign in again to refresh the authentication cookie
                await _signInManager.SignOutAsync();
                await _signInManager.SignInAsync(user, isPersistent: false);
                
                TempData["Success"] = "Password changed successfully!";
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SwitchLearningPath(string learningPath)
        {
            var allowed = new[] { "Python", "Java" };
            if (!allowed.Contains(learningPath))
            {
                TempData["Error"] = "Invalid learning path selected.";
                return RedirectToAction("Index");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "User");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return RedirectToAction("Login", "User");

            user.LearningPath = learningPath;
            user.PreferredLanguage = learningPath;
            await _userManager.UpdateAsync(user);

            TempData["Success"] = $"Learning path switched to {learningPath} successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                TempData["Success"] = "If an account with that email exists, a password reset link has been sent.";
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            // Generate password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            // For development: Show the reset link. In production, this should be sent via email
            var callbackUrl = Url.Action("ResetPassword", "Settings", new { email = user.Email, token = token }, protocol: Request.Scheme);
            
            TempData["ResetLink"] = callbackUrl;
            TempData["Email"] = user.Email;
            
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string? email, string? token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "User");
            }

            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                TempData["Success"] = "Password has been reset. You can now log in with your new password.";
                return RedirectToAction("Login", "User");
            }

            // Reset password
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["Success"] = "Password has been reset successfully. You can now log in with your new password.";
                return RedirectToAction("Login", "User");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
    }
}

