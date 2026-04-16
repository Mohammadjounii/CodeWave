using CodeWave.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IO;
using System.Text.Json;
using CodeWave.Infrastructure.Services;
using CodeWave.Application.Interfaces;

namespace CodeWave.Web.Controllers
{
    [Authorize]
    public class CvController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICVService _cvService;
        private readonly ICVPDFService _pdfService;
        private readonly IProgressService _progressService;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserCourseRepository _userCourseRepository;

        public CvController(
            UserManager<ApplicationUser> userManager, 
            ICVService cvService, 
            ICVPDFService pdfService, 
            IProgressService progressService,
            IProjectRepository projectRepository,
            IUserCourseRepository userCourseRepository)
        {
            _userManager = userManager;
            _cvService = cvService;
            _pdfService = pdfService;
            _progressService = progressService;
            _projectRepository = projectRepository;
            _userCourseRepository = userCourseRepository;
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

            // Check if user is Beginner - lock access
            if (!string.IsNullOrEmpty(user.Level) && user.Level.Equals("Beginner", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "CV section is only available for Intermediate and Advanced users. Complete more assessments to unlock this feature.";
                return RedirectToAction("Index", "Home");
            }

            // Build full name
            var fullName = user.FirstName;
            if (!string.IsNullOrEmpty(user.LastName))
            {
                fullName += " " + user.LastName;
            }

            // Get level or default to "Developer"
            var level = !string.IsNullOrEmpty(user.Level) ? user.Level + " Developer" : "Developer";

            ViewBag.UserName = fullName;
            ViewBag.UserFirstName = user.FirstName;
            ViewBag.UserLevel = level;
            ViewBag.LearningPath = user.LearningPath; // For Dashboard link redirection
            ViewBag.UserLevelRaw = user.Level; // Raw level for checking if Beginner
            ViewBag.ProfilePictureUrl = user.ProfilePictureUrl; // Profile picture URL

            // Get or create CV for user
            var cv = await _cvService.GetCVByUserIdAsync(userGuid);

            ViewBag.CV = cv;
            ViewBag.UserEmail = user.Email;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveCV(
            string fullName,
            int? age,
            string location,
            string email,
            string phone,
            string linkedInUrl,
            string githubUrl,
            string education,
            string educationDegree,
            string educationDate,
            string educationDetails,
            string programmingLanguages,
            string spokenLanguages,
            string summary,
            string experience,
            string certifications,
            string projects,
            IFormFile cvPicture,
            string template = "dublin")
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
                TempData["Error"] = "CV section is only available for Intermediate and Advanced users.";
                return RedirectToAction("Index", "Home");
            }

            var cv = await _cvService.GetCVByUserIdAsync(userGuid);

            if (cv == null)
            {
                cv = new CV
                {
                    Id = Guid.NewGuid(),
                    UserId = userGuid,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow,
                    Skills = string.Empty,  // Initialize Skills to empty string to satisfy database constraint
                    Template = !string.IsNullOrEmpty(template) ? template : "dublin",  // Initialize template
                    FullName = string.Empty,
                    Email = string.Empty
                };
            }
            else
            {
                // Ensure CreatedAt is set for existing CVs
                if (cv.CreatedAt == default(DateTime))
                {
                    cv.CreatedAt = DateTime.UtcNow;
                }
                // Ensure Skills is set for existing CVs (if null, set to empty string)
                if (string.IsNullOrEmpty(cv.Skills))
                {
                    cv.Skills = string.Empty;
                }
            }

            // Update CV fields
            cv.FullName = fullName ?? string.Empty;
            cv.Age = age;
            cv.Location = location ?? string.Empty;
            cv.Email = email ?? user.Email ?? string.Empty;
            cv.Phone = phone ?? string.Empty;
            cv.LinkedInUrl = linkedInUrl ?? string.Empty;
            cv.GitHubUrl = githubUrl ?? string.Empty;
            cv.Education = education ?? string.Empty;
            
            // Combine education details with degree and date
            var educationDetailsList = new List<string>();
            if (!string.IsNullOrEmpty(educationDegree))
            {
                educationDetailsList.Add(educationDegree);
            }
            if (!string.IsNullOrEmpty(educationDate))
            {
                var date = DateTime.TryParse(educationDate + "-01", out var parsedDate) 
                    ? parsedDate.ToString("MMMM yyyy") 
                    : educationDate;
                educationDetailsList.Add($"Graduated: {date}");
            }
            if (!string.IsNullOrEmpty(educationDetails))
            {
                educationDetailsList.Add(educationDetails);
            }
            cv.EducationDetails = string.Join("\n", educationDetailsList);
            cv.ProgrammingLanguages = programmingLanguages ?? string.Empty;
            cv.SpokenLanguages = spokenLanguages ?? string.Empty;
            cv.Summary = summary ?? string.Empty;
            
            // Handle Experience - can be JSON or plain text (for backward compatibility)
            if (!string.IsNullOrWhiteSpace(experience))
            {
                // Try to parse as JSON, if fails, store as plain text
                try
                {
                    var expJson = JsonSerializer.Deserialize<object>(experience);
                    cv.Experience = experience; // Store as JSON
                }
                catch
                {
                    cv.Experience = experience; // Store as plain text
                }
            }
            else
            {
                // Save empty array as JSON if no experience provided
                cv.Experience = "[]";
            }
            
            // Handle Certifications - can be JSON or plain text (for backward compatibility)
            if (!string.IsNullOrWhiteSpace(certifications))
            {
                // Try to parse as JSON, if fails, store as plain text
                try
                {
                    var certJson = JsonSerializer.Deserialize<object>(certifications);
                    cv.Certifications = certifications; // Store as JSON
                }
                catch
                {
                    cv.Certifications = certifications; // Store as plain text
                }
            }
            else
            {
                // Save empty array as JSON if no certifications provided
                cv.Certifications = "[]";
            }
            
            // Handle Projects - can be JSON or plain text (for backward compatibility)
            if (!string.IsNullOrWhiteSpace(projects))
            {
                // Try to parse as JSON, if fails, store as plain text
                try
                {
                    var projJson = JsonSerializer.Deserialize<object>(projects);
                    cv.Projects = projects; // Store as JSON
                }
                catch
                {
                    cv.Projects = projects; // Store as plain text
                }
            }
            else
            {
                // Save empty array as JSON if no projects provided
                cv.Projects = "[]";
            }
            cv.Template = !string.IsNullOrEmpty(template) ? template : "dublin";
            cv.Skills = string.Empty; // Ensure Skills field is set
            cv.LastUpdated = DateTime.UtcNow;

            // Save CV using service
            cv = await _cvService.CreateOrUpdateCVAsync(cv);

            // Handle CV picture upload
            if (cvPicture != null && cvPicture.Length > 0)
            {
                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(cvPicture.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(fileExtension))
                {
                    TempData["Error"] = "Invalid file type. Please upload a JPG, PNG, or GIF image.";
                    return RedirectToAction("Index");
                }

                // Validate file size (max 5MB)
                if (cvPicture.Length > 5 * 1024 * 1024)
                {
                    TempData["Error"] = "File size too large. Please upload an image smaller than 5MB.";
                    return RedirectToAction("Index");
                }

                // Create uploads directory if it doesn't exist
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "cv-pictures");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate unique filename
                var fileName = $"{userGuid}_{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await cvPicture.CopyToAsync(stream);
                }

                // Delete old CV picture if exists
                if (!string.IsNullOrEmpty(cv.CVPictureUrl))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", cv.CVPictureUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Update CV picture URL
                cv.CVPictureUrl = $"/uploads/cv-pictures/{fileName}";
            }

            // Generate PDF CV before saving
            string relativePdfPath = null;
            try
            {
                // Ensure required fields are not null for PDF generation
                if (string.IsNullOrEmpty(cv.FullName))
                {
                    cv.FullName = "Your Name";
                }
                if (string.IsNullOrEmpty(cv.Email))
                {
                    cv.Email = user.Email ?? "your.email@example.com";
                }

                // Ensure wwwroot directory exists
                var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                if (!Directory.Exists(wwwrootPath))
                {
                    Directory.CreateDirectory(wwwrootPath);
                }

                var pdfFolder = Path.Combine(wwwrootPath, "uploads", "cv-pdfs");
                if (!Directory.Exists(pdfFolder))
                {
                    Directory.CreateDirectory(pdfFolder);
                }

                var pdfFileName = $"{userGuid}_cv_{Guid.NewGuid()}.pdf";
                var pdfPath = Path.Combine(pdfFolder, pdfFileName);
                relativePdfPath = $"/uploads/cv-pdfs/{pdfFileName}";

                // Generate PDF
                await _pdfService.GenerateCVPDFAsync(cv, pdfPath);

                // Verify PDF was created and has content
                if (!System.IO.File.Exists(pdfPath))
                {
                    throw new Exception($"PDF file was not created at: {pdfPath}");
                }

                var fileInfo = new FileInfo(pdfPath);
                if (fileInfo.Length == 0)
                {
                    throw new Exception("PDF file was created but is empty (0 bytes).");
                }

                // Delete old PDF if exists
                if (!string.IsNullOrEmpty(cv.GeneratedPDFPath))
                {
                    var oldPdfPath = Path.Combine(wwwrootPath, cv.GeneratedPDFPath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPdfPath))
                    {
                        try
                        {
                            System.IO.File.Delete(oldPdfPath);
                        }
                        catch (Exception deleteEx)
                        {
                            // Log but don't fail if old file can't be deleted
                            System.Diagnostics.Debug.WriteLine($"Could not delete old PDF: {deleteEx.Message}");
                        }
                    }
                }

                // Update CV with PDF path
                cv.GeneratedPDFPath = relativePdfPath;
                
                // Save CV again to persist the PDF path to database
                cv = await _cvService.CreateOrUpdateCVAsync(cv);
            }
            catch (Exception ex)
            {
                // Log the full exception for debugging
                var errorMessage = $"PDF generation failed: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $" Inner: {ex.InnerException.Message}";
                }
                
                // Only show first few lines of stack trace to user
                var stackTraceLines = ex.StackTrace?.Split('\n').Take(3);
                if (stackTraceLines != null && stackTraceLines.Any())
                {
                    errorMessage += $" Location: {string.Join(" ", stackTraceLines)}";
                }
                
                TempData["Error"] = errorMessage;
                
                // Still save the CV even if PDF generation fails
                // User can regenerate PDF later by resubmitting the form
            }

            // Check for errors and show success message
            if (string.IsNullOrEmpty(TempData["Error"] as string))
            {
                TempData["Success"] = "CV saved and PDF generated successfully!";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            if (profilePicture != null && profilePicture.Length > 0)
            {
                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(profilePicture.FileName).ToLowerInvariant();
                
                if (!allowedExtensions.Contains(fileExtension))
                {
                    TempData["Error"] = "Invalid file type. Please upload a JPG, PNG, or GIF image.";
                    return RedirectToAction("Index");
                }

                // Validate file size (max 5MB)
                if (profilePicture.Length > 5 * 1024 * 1024)
                {
                    TempData["Error"] = "File size too large. Please upload an image smaller than 5MB.";
                    return RedirectToAction("Index");
                }

                // Create uploads directory if it doesn't exist
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profile-pictures");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generate unique filename
                var fileName = $"{userGuid}_{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(stream);
                }

                // Delete old profile picture if exists
                if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfilePictureUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Update user's profile picture URL
                user.ProfilePictureUrl = $"/uploads/profile-pictures/{fileName}";
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["Success"] = "Profile picture updated successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to update profile picture.";
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadCVFile(IFormFile cvFile)
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
                TempData["Error"] = "CV section is only available for Intermediate and Advanced users.";
                return RedirectToAction("Index", "Home");
            }

            if (cvFile == null || cvFile.Length == 0)
            {
                TempData["Error"] = "Please select a CV file to upload.";
                return RedirectToAction("Index");
            }

            // Validate file type
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
            var fileExtension = Path.GetExtension(cvFile.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(fileExtension))
            {
                TempData["Error"] = "Invalid file type. Please upload a PDF, DOC, or DOCX file.";
                return RedirectToAction("Index");
            }

            // Validate file size (max 10MB)
            if (cvFile.Length > 10 * 1024 * 1024)
            {
                TempData["Error"] = "File size too large. Please upload a file smaller than 10MB.";
                return RedirectToAction("Index");
            }

            // Get or create CV
            var cv = await _cvService.GetCVByUserIdAsync(userGuid);

            if (cv == null)
            {
                cv = new CV
                {
                    Id = Guid.NewGuid(),
                    UserId = userGuid,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow
                };
            }

            // Create uploads directory if it doesn't exist
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "cv-files");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate unique filename
            var fileName = $"{userGuid}_{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await cvFile.CopyToAsync(stream);
            }

            // Delete old CV file if exists
            if (!string.IsNullOrEmpty(cv.UploadedCVFilePath))
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", cv.UploadedCVFilePath.TrimStart('/'));
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            // Update CV file path
            cv.UploadedCVFilePath = $"/uploads/cv-files/{fileName}";
            cv.LastUpdated = DateTime.UtcNow;
            await _cvService.CreateOrUpdateCVAsync(cv);

            TempData["Success"] = "CV file uploaded successfully! You can now upgrade it to a professional CV.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpgradeCV(string template = "dublin")
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
                TempData["Error"] = "CV section is only available for Intermediate and Advanced users.";
                return RedirectToAction("Index", "Home");
            }

            var cv = await _cvService.GetCVByUserIdAsync(userGuid);

            if (cv == null || string.IsNullOrEmpty(cv.UploadedCVFilePath))
            {
                TempData["Error"] = "Please upload a CV file first before upgrading.";
                return RedirectToAction("Index");
            }

            try
            {
                // For now, we'll copy the uploaded file as the "upgraded" version
                // In a real implementation, you would process the CV file, extract content,
                // improve formatting, add professional sections, etc.
                // This could involve:
                // 1. PDF/DOCX parsing libraries
                // 2. AI services for content enhancement
                // 3. Template application
                // 4. Formatting improvements

                var uploadedFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", cv.UploadedCVFilePath.TrimStart('/'));
                
                if (!System.IO.File.Exists(uploadedFilePath))
                {
                    TempData["Error"] = "Uploaded CV file not found. Please upload again.";
                    return RedirectToAction("Index");
                }

                // Create upgraded CVs directory
                var upgradedFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "cv-upgraded");
                if (!Directory.Exists(upgradedFolder))
                {
                    Directory.CreateDirectory(upgradedFolder);
                }

                // Generate upgraded filename
                var fileExtension = Path.GetExtension(cv.UploadedCVFilePath);
                var upgradedFileName = $"{userGuid}_upgraded_{Guid.NewGuid()}{fileExtension}";
                var upgradedFilePath = Path.Combine(upgradedFolder, upgradedFileName);

                // Copy file as upgraded version (in real implementation, this would be processed)
                System.IO.File.Copy(uploadedFilePath, upgradedFilePath, true);

                // Delete old upgraded CV if exists
                if (!string.IsNullOrEmpty(cv.UpgradedCVFilePath))
                {
                    var oldUpgradedPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", cv.UpgradedCVFilePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldUpgradedPath))
                    {
                        System.IO.File.Delete(oldUpgradedPath);
                    }
                }

                // Update CV with upgraded file path and template
                cv.UpgradedCVFilePath = $"/uploads/cv-upgraded/{upgradedFileName}";
                cv.Template = !string.IsNullOrEmpty(template) ? template : "dublin";
                cv.LastUpdated = DateTime.UtcNow;
                await _cvService.CreateOrUpdateCVAsync(cv);

                // Generate professional PDF CV
                try
                {
                    var pdfFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "cv-pdfs");
                    if (!Directory.Exists(pdfFolder))
                    {
                        Directory.CreateDirectory(pdfFolder);
                    }

                    var pdfFileName = $"{userGuid}_cv_upgraded_{Guid.NewGuid()}.pdf";
                    var pdfPath = Path.Combine(pdfFolder, pdfFileName);
                    var relativePdfPath = $"/uploads/cv-pdfs/{pdfFileName}";

                    // Generate PDF
                    await _pdfService.GenerateCVPDFAsync(cv, pdfPath);

                    // Delete old PDF if exists
                    if (!string.IsNullOrEmpty(cv.GeneratedPDFPath))
                    {
                        var oldPdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", cv.GeneratedPDFPath.TrimStart('/'));
                        if (System.IO.File.Exists(oldPdfPath))
                        {
                            System.IO.File.Delete(oldPdfPath);
                        }
                    }

                    // Update CV with PDF path
                    cv.GeneratedPDFPath = relativePdfPath;
                    await _cvService.CreateOrUpdateCVAsync(cv);

                    TempData["Success"] = "CV upgraded successfully! Your professional PDF CV is ready for download.";
                }
                catch (Exception pdfEx)
                {
                    TempData["Success"] = "CV upgraded successfully, but PDF generation failed. Please try again.";
                    // Log error if needed
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error upgrading CV: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DownloadPDF()
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

            var cv = await _cvService.GetCVByUserIdAsync(userGuid);

            if (cv == null || string.IsNullOrEmpty(cv.GeneratedPDFPath))
            {
                TempData["Error"] = "CV PDF not found. Please generate your CV first.";
                return RedirectToAction("Index");
            }

            var pdfPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", cv.GeneratedPDFPath.TrimStart('/'));
            if (!System.IO.File.Exists(pdfPath))
            {
                TempData["Error"] = "PDF file not found.";
                return RedirectToAction("Index");
            }

            // Get user's full name
            var fullName = cv.FullName ?? $"{user.FirstName} {user.LastName}".Trim();
            if (string.IsNullOrEmpty(fullName))
            {
                fullName = user.Email?.Split('@')[0] ?? "User";
            }

            // Sanitize filename (remove invalid characters)
            fullName = string.Join("_", fullName.Split(Path.GetInvalidFileNameChars()));

            // Determine filename based on user level
            string fileName;
            var userLevel = user.Level ?? "";
            if (userLevel.Equals("Intermediate", StringComparison.OrdinalIgnoreCase))
            {
                fileName = $"{fullName} - Resume.pdf";
            }
            else if (userLevel.Equals("Advanced", StringComparison.OrdinalIgnoreCase) || 
                     userLevel.Equals("Pro", StringComparison.OrdinalIgnoreCase))
            {
                fileName = $"{fullName} - CV.pdf";
            }
            else
            {
                // Default for other levels
                fileName = $"{fullName} - Resume.pdf";
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(pdfPath);
            return File(fileBytes, "application/pdf", fileName);
        }

        public async Task<IActionResult> ViewCV()
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
                TempData["Error"] = "CV section is only available for Intermediate and Advanced users.";
                return RedirectToAction("Index", "Home");
            }

            // Build full name
            var fullName = user.FirstName;
            if (!string.IsNullOrEmpty(user.LastName))
            {
                fullName += " " + user.LastName;
            }

            // Get level or default to "Developer"
            var level = !string.IsNullOrEmpty(user.Level) ? user.Level + " Developer" : "Developer";

            ViewBag.UserName = fullName;
            ViewBag.UserFirstName = user.FirstName;
            ViewBag.UserLevel = level;
            ViewBag.LearningPath = user.LearningPath;
            ViewBag.UserLevelRaw = user.Level;
            ViewBag.ProfilePictureUrl = user.ProfilePictureUrl;
            ViewBag.UserEmail = user.Email;

            // Get CV for user
            var cv = await _cvService.GetCVByUserIdAsync(userGuid);

            ViewBag.CV = cv;

            // Get completed courses
            var completedCourses = await _userCourseRepository.GetCompletedCoursesByUserIdAsync(userGuid);

            ViewBag.CompletedCourses = completedCourses;

            // Get user projects
            var userProjects = await _projectRepository.GetUserProjectsAsync(userGuid);

            ViewBag.UserProjects = userProjects;

            return View("View");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AutoFillCV()
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
                TempData["Error"] = "CV section is only available for Intermediate and Advanced users.";
                return RedirectToAction("Index", "Home");
            }

            // Get or create CV
            var cv = await _cvService.GetCVByUserIdAsync(userGuid);

            if (cv == null)
            {
                cv = new CV
                {
                    Id = Guid.NewGuid(),
                    UserId = userGuid,
                    CreatedAt = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow,
                    Skills = string.Empty,
                    Template = "dublin",
                    FullName = string.Empty,
                    Email = string.Empty
                };
                // CV will be saved by CreateOrUpdateCVAsync below
            }

            // Preserve existing education and experience
            var existingEducation = cv.Education ?? string.Empty;
            var existingEducationDetails = cv.EducationDetails ?? string.Empty;
            var existingExperience = cv.Experience ?? string.Empty;

            // Build full name
            var fullName = user.FirstName;
            if (!string.IsNullOrEmpty(user.LastName))
            {
                fullName += " " + user.LastName;
            }

            // Get level or default to "Developer"
            var level = !string.IsNullOrEmpty(user.Level) ? user.Level + " Developer" : "Developer";

            // Auto-fill Personal Information
            cv.FullName = fullName;
            cv.Email = user.Email ?? string.Empty;
            cv.CVPictureUrl = cv.CVPictureUrl ?? user.ProfilePictureUrl; // Use existing CV picture or profile picture

            // Generate Professional Summary from user data
            var summaryParts = new List<string>();
            
            if (!string.IsNullOrEmpty(user.Level))
            {
                summaryParts.Add($"A {user.Level.ToLower()} developer");
            }
            else
            {
                summaryParts.Add("A motivated developer");
            }

            if (!string.IsNullOrEmpty(user.LearningPath))
            {
                summaryParts.Add($"specializing in {user.LearningPath}");
            }

            if (!string.IsNullOrEmpty(user.Interests))
            {
                summaryParts.Add($"with a passion for {user.Interests.ToLower()}");
            }

            if (!string.IsNullOrEmpty(user.Goal))
            {
                summaryParts.Add($"aiming to {user.Goal.ToLower()}");
            }

            if (!string.IsNullOrEmpty(user.Motivation))
            {
                summaryParts.Add($"driven by {user.Motivation.ToLower()}");
            }

            // Get user progress to add achievements
            var userProgress = await _progressService.GetUserProgressAsync(userGuid);
            if (userProgress != null && userProgress.OverallProgressPercent > 0)
            {
                summaryParts.Add($"with {Math.Round(userProgress.OverallProgressPercent, 0)}% completion across {userProgress.CompletedLessons} lessons, {userProgress.CompletedExercises} exercises, and {userProgress.PassedQuizzes} passed quizzes");
            }

            summaryParts.Add("Eager to leverage skills from completed courses in a challenging professional environment.");

            cv.Summary = string.Join(". ", summaryParts) + ".";

            // Generate Skills from Learning Path and Progress
            var skillsList = new List<string>();
            
            // Get user skills from progress service
            if (!string.IsNullOrEmpty(user.LearningPath))
            {
                var userSkills = await _progressService.GetUserSkillsAsync(userGuid, user.LearningPath);
                foreach (var skill in userSkills.Where(s => s.MasteryLevel > 0))
                {
                    skillsList.Add(skill.SkillName);
                }
            }

            // Add learning path specific technologies
            var learningPathLower = (user.LearningPath ?? "").ToLower();
            if (learningPathLower.Contains("python"))
            {
                skillsList.AddRange(new[] { "Python", "Object-Oriented Programming", "Data Structures", "Algorithms" });
            }
            else if (learningPathLower.Contains("java"))
            {
                skillsList.AddRange(new[] { "Java", "Object-Oriented Programming", "Collections Framework", "Exception Handling" });
            }
            else if (learningPathLower.Contains("web"))
            {
                skillsList.AddRange(new[] { "HTML5", "CSS3", "JavaScript", "React", "Node.js", "REST APIs" });
            }

            // Add common skills
            skillsList.AddRange(new[] { "Git & GitHub", "Problem Solving", "Code Review" });

            // Remove duplicates and format
            cv.ProgrammingLanguages = string.Join(", ", skillsList.Distinct());

            // Generate Projects from User Projects
            var userProjects = await _projectRepository.GetUserProjectsAsync(userGuid);

            if (userProjects.Any())
            {
                var projectsText = new List<string>();
                foreach (var project in userProjects.Take(5)) // Limit to 5 projects
                {
                    var projectEntry = new List<string> { project.Title ?? "Project" };
                    
                    if (!string.IsNullOrEmpty(project.Description))
                    {
                        projectEntry.Add($"Description: {project.Description}");
                    }
                    
                    if (!string.IsNullOrEmpty(project.Result))
                    {
                        projectEntry.Add($"Technologies: {project.Result}");
                    }
                    
                    projectsText.Add(string.Join("\n", projectEntry));
                }
                cv.Projects = string.Join("\n\n", projectsText);
            }

            // Generate Certifications from Completed Courses
            var completedCourses = await _userCourseRepository.GetCompletedCoursesByUserIdAsync(userGuid);

            if (completedCourses.Any())
            {
                var certificationsList = new List<string>();
                foreach (var course in completedCourses.OrderByDescending(c => c.CreatedAt))
                {
                    var completionDate = await _userCourseRepository.GetCourseCompletionDateAsync(userGuid, course.Id);

                    var dateStr = completionDate.HasValue 
                        ? completionDate.Value.ToString("MMMM yyyy") 
                        : course.CreatedAt.ToString("MMMM yyyy");

                    certificationsList.Add($"{course.Title} - CodeWave - Completed, {dateStr}");
                }
                cv.Certifications = string.Join("\n", certificationsList);
            }

            // Preserve existing education and experience
            cv.Education = existingEducation;
            cv.EducationDetails = existingEducationDetails;
            cv.Experience = existingExperience;

            // Update last updated
            cv.LastUpdated = DateTime.UtcNow;

            // Save changes
            await _cvService.CreateOrUpdateCVAsync(cv);

            TempData["Success"] = "CV has been auto-filled with your profile data! You can now edit Education and Experience sections.";
            return RedirectToAction("Index");
        }
    }
}
