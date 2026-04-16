using CodeWave.Domain.Entities;
using CodeWave.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Linq;

namespace CodeWave.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                // Redirect admins to Admin dashboard
                if (user.IsAdmin)
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName, // Can be null
                Description = null, // Will be set later by user
                Level = null, // Will be set after assessment
                LearningPath = null // Will be set after assessment
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                // Redirect new users to onboarding flow
                return RedirectToAction("UserInterest", "Welcome");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Welcome");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GoogleLogin(string? returnUrl = null)
        {
            var redirectUrl = Url.Action("GoogleCallback", "User", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(properties, "Google");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleCallback(string? returnUrl = null, string? remoteError = null)
        {
            if (remoteError != null)
            {
                TempData["Error"] = $"Error from external provider: {remoteError}";
                return RedirectToAction("Login");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["Error"] = "Error loading external login information.";
                return RedirectToAction("Login");
            }

            // Get email once at the beginning
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                // Get the logged-in user to check if they've completed onboarding
                if (!string.IsNullOrEmpty(email))
                {
                    var existingUser = await _userManager.FindByEmailAsync(email);
                    if (existingUser != null)
                    {
                        // Check if admin, redirect to admin dashboard
                        if (existingUser.IsAdmin)
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        
                        if (string.IsNullOrEmpty(existingUser.Level))
                        {
                            // New user, redirect to onboarding
                            return RedirectToAction("UserInterest", "Welcome");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                TempData["Error"] = "Your account is locked out.";
                return RedirectToAction("Login");
            }

            // User doesn't have an account, create one
            var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? info.Principal.FindFirstValue(ClaimTypes.Name)?.Split(' ')[0] ?? "User";
            var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? info.Principal.FindFirstValue(ClaimTypes.Name)?.Split(' ').LastOrDefault() ?? "";

            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Could not retrieve email from Google account.";
                return RedirectToAction("Login");
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = firstName,
                LastName = lastName,
                Description = null,
                Level = null,
                LearningPath = null
            };

            var createResult = await _userManager.CreateAsync(user);
            if (createResult.Succeeded)
            {
                var addLoginResult = await _userManager.AddLoginAsync(user, info);
                if (addLoginResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    // New user from Google, redirect to onboarding
                    return RedirectToAction("UserInterest", "Welcome");
                }
            }

            TempData["Error"] = "Failed to create account. Please try again.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GitHubLogin(string? returnUrl = null)
        {
            // This URL is where we want to go AFTER the external sign-in finishes
            var redirectUrl = Url.Action("GitHubCallback", "User", new { returnUrl });

            // This configures the external login (creates the state/correlation cookie)
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("GitHub", redirectUrl);

            // This sends the user to GitHub
            return Challenge(properties, "GitHub");
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GitHubCallback(string? returnUrl = null, string? remoteError = null)
        {
            if (remoteError != null)
            {
                TempData["Error"] = $"Error from GitHub: {remoteError}";
                return RedirectToAction("Login");
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["Error"] = "Error loading GitHub login information.";
                return RedirectToAction("Login");
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false);

            // Get email first
            var email = info.Principal.FindFirstValue(ClaimTypes.Email)
                ?? info.Principal.FindFirstValue("email")
                ?? $"{Guid.NewGuid()}@github.local";

            if (signInResult.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                // Check if user is admin
                var existingUser = await _userManager.FindByEmailAsync(email);
                if (existingUser != null && existingUser.IsAdmin)
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }

            var name = info.Principal.FindFirstValue(ClaimTypes.Name)
                ?? info.Principal.FindFirstValue("login")
                ?? "GitHub User";

            var firstName = name.Split(' ')[0];
            var lastName = name.Split(' ').Length > 1
                ? string.Join(" ", name.Split(' ').Skip(1))
                : "";

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = firstName,
                LastName = lastName,
                Description = null,
                Level = null,
                LearningPath = null
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                TempData["Error"] = "Failed to create user from GitHub account.";
                return RedirectToAction("Login");
            }

            var addLoginResult = await _userManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded)
            {
                TempData["Error"] = "Failed to link GitHub login.";
                return RedirectToAction("Login");
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            // New user from GitHub, redirect to onboarding
            return RedirectToAction("UserInterest", "Welcome");
        }

    }
}
