using CodeWave.Domain.Entities;
using CodeWave.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CodeWave.Application.Interfaces;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace CodeWave.Web.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        private readonly ILearningPathService _learningPathService;
        private readonly IUserReportService _reportService;
        private readonly IEmailService _emailService;
        private readonly ILogger<SettingsController> _logger;

        public SettingsController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserService userService,
            ILearningPathService learningPathService,
            IUserReportService reportService,
            IEmailService emailService,
            ILogger<SettingsController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _learningPathService = learningPathService;
            _reportService = reportService;
            _emailService = emailService;
            _logger = logger;
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
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["PasswordError"] = string.Join(" ", errors);
                return RedirectToAction("Index");
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
            var allowed = new[] { "Python", "Java", "Web Development" };
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

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(token));
            var callbackUrl = Url.Action("ResetPassword", "Settings", new { email = user.Email, token = encodedToken }, protocol: Request.Scheme);

            var htmlBody = $@"
                <div style='font-family:Arial,sans-serif;max-width:600px;margin:auto;padding:30px;border:1px solid #e0e0e0;border-radius:8px;'>
                    <h2 style='color:#b887e3;'>CodeWave — Password Reset</h2>
                    <p>Hello,</p>
                    <p>We received a request to reset your password. Click the button below to reset it:</p>
                    <div style='text-align:center;margin:30px 0;'>
                        <a href='{callbackUrl}' style='background:#b887e3;color:#fff;padding:14px 28px;border-radius:8px;text-decoration:none;font-weight:bold;font-size:16px;'>Reset Password</a>
                    </div>
                    <p>If you did not request a password reset, you can safely ignore this email.</p>
                    <p style='color:#888;font-size:12px;margin-top:30px;'>This link will expire after 24 hours. &mdash; The CodeWave Team</p>
                </div>";

            try
            {
                await _emailService.SendEmailAsync(user.Email!, "Reset your CodeWave password", htmlBody);
                TempData["Success"] = "A password reset link has been sent to your email.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send password reset email to {Email}", user.Email);
                TempData["ResetLink"] = callbackUrl;
                TempData["Success"] = "Could not send email. Use this link to reset your password:";
            }

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

            var decodedToken = System.Text.Encoding.UTF8.GetString(Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlDecode(model.Token));
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReport(string category, string subject, string description)
        {
            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(description))
            {
                TempData["ReportError"] = "Please fill in all required fields.";
                return RedirectToAction("Index");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId ?? "");
            if (user == null) return RedirectToAction("Login", "User");

            var report = new UserReport
            {
                UserId = Guid.Parse(userId!),
                UserName = $"{user.FirstName} {user.LastName}".Trim(),
                UserEmail = user.Email ?? "",
                Category = category ?? "General",
                Subject = subject.Trim(),
                Description = description.Trim()
            };

            await _reportService.SubmitReportAsync(report);
            TempData["ReportSuccess"] = "Your report has been submitted. Thank you for your feedback!";
            return RedirectToAction("Index");
        }
    }
}

