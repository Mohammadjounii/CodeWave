using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using CodeWave.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CodeWave.Web.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ============================================
        // DASHBOARD / INDEX
        // ============================================
        public async Task<IActionResult> Index(int days = 30)
        {
            var totalUsers = await _context.Users.CountAsync();
            var totalCourses = await _context.Courses.Where(c => !c.IsDeleted).CountAsync();
            var totalApplications = await _context.ExternalJobApplications.CountAsync();
            var recentActivity = await GetRecentActivityAsync();

            // Calculate user engagement data for the selected period
            var startDate = DateTime.UtcNow.AddDays(-days);
            var engagementData = await GetUserEngagementDataAsync(startDate, days);
            
            // Calculate skill distribution
            var skillDistribution = await GetSkillDistributionAsync();
            
            // Get top courses
            var topCourses = await GetTopCoursesAsync();

            // Recent users for dashboard panel
            var recentUsers = await _context.Users
                .OrderByDescending(u => u.Id)
                .Take(10)
                .Select(u => new
                {
                    u.Id,
                    FirstName = u.FirstName ?? "",
                    LastName  = u.LastName  ?? "",
                    Email     = u.Email     ?? "",
                    LearningPath = u.LearningPath ?? "None",
                    Level    = u.Level    ?? "New",
                    u.IsAdmin
                })
                .ToListAsync();

            // Learning path distribution
            var userPathInfo = await _context.Users
                .Select(u => new { u.Id, Path = u.LearningPath ?? "None" })
                .ToListAsync();

            var activeUserIds = await _context.LessonCompletions
                .Where(lc => !lc.isDeleted)
                .Select(lc => lc.UserId)
                .Distinct()
                .ToListAsync();
            var activeSet = new HashSet<Guid>(activeUserIds);

            var pathDistribution = new List<object>();
            foreach (var grp in userPathInfo
                .GroupBy(u => string.IsNullOrEmpty(u.Path) ? "None" : u.Path)
                .OrderByDescending(g => g.Count()))
            {
                var cnt    = grp.Count();
                var active = grp.Count(u => activeSet.Contains(u.Id));
                pathDistribution.Add(new
                {
                    Path    = grp.Key,
                    Total   = cnt,
                    Active  = active,
                    Percent = totalUsers > 0 ? Math.Round(cnt / (double)totalUsers * 100, 1) : 0.0
                });
            }

            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalCourses = totalCourses;
            ViewBag.TotalApplications = totalApplications;
            ViewBag.RecentActivity = recentActivity;
            ViewBag.EngagementData = engagementData;
            ViewBag.SkillDistribution = skillDistribution;
            ViewBag.TopCourses = topCourses;
            ViewBag.SelectedDays = days;
            ViewBag.RecentUsers = recentUsers.Cast<object>().ToList();
            ViewBag.PathDistribution = pathDistribution;

            return View();
        }

        private async Task<List<object>> GetUserEngagementDataAsync(DateTime startDate, int days)
        {
            var engagementData = new List<object>();
            
            for (int i = 0; i < days; i++)
            {
                var date = startDate.AddDays(i).Date;
                var nextDate = date.AddDays(1);

                // Count unique users who had activity on this day
                var lessonCompletions = await _context.LessonCompletions
                    .Where(lc => !lc.isDeleted && 
                        (lc.CompletionDate.HasValue && lc.CompletionDate.Value.Date == date) ||
                        (!lc.CompletionDate.HasValue && lc.CreatedAt.Date == date))
                    .Select(lc => lc.UserId)
                    .Distinct()
                    .CountAsync();

                var exerciseSubmissions = await _context.ExerciseSubmissions
                    .Where(es => !es.isDeleted && es.SubmissionDate.Date == date)
                    .Select(es => es.UserId)
                    .Distinct()
                    .CountAsync();

                var quizAttempts = await _context.UserQuizAttempts
                    .Where(qa => !qa.IsDeleted && qa.StartedAt.Date == date)
                    .Select(qa => qa.UserId)
                    .Distinct()
                    .CountAsync();

                // Get unique users across all activity types
                var uniqueUsers = await _context.LessonCompletions
                    .Where(lc => !lc.isDeleted && 
                        ((lc.CompletionDate.HasValue && lc.CompletionDate.Value.Date == date) ||
                        (!lc.CompletionDate.HasValue && lc.CreatedAt.Date == date)))
                    .Select(lc => lc.UserId)
                    .Union(_context.ExerciseSubmissions
                        .Where(es => !es.isDeleted && es.SubmissionDate.Date == date)
                        .Select(es => es.UserId))
                    .Union(_context.UserQuizAttempts
                        .Where(qa => !qa.IsDeleted && qa.StartedAt.Date == date)
                        .Select(qa => qa.UserId))
                    .Distinct()
                    .CountAsync();

                engagementData.Add(new
                {
                    Date = date.ToString("MMM dd"),
                    DateValue = date.ToString("yyyy-MM-dd"),
                    Sessions = uniqueUsers,
                    LessonCompletions = lessonCompletions,
                    ExerciseSubmissions = exerciseSubmissions,
                    QuizAttempts = quizAttempts
                });
            }

            return engagementData;
        }

        private async Task<Dictionary<string, double>> GetSkillDistributionAsync()
        {
            var users = await _context.Users
                .Where(u => !string.IsNullOrEmpty(u.Level))
                .Select(u => u.Level)
                .ToListAsync();

            var total = users.Count;
            if (total == 0) return new Dictionary<string, double>
            {
                { "Beginner", 0 },
                { "Intermediate", 0 },
                { "Advanced", 0 }
            };

            return new Dictionary<string, double>
            {
                { "Beginner", Math.Round((users.Count(l => l.Contains("Beginner", StringComparison.OrdinalIgnoreCase)) / (double)total) * 100, 1) },
                { "Intermediate", Math.Round((users.Count(l => l.Contains("Intermediate", StringComparison.OrdinalIgnoreCase)) / (double)total) * 100, 1) },
                { "Advanced", Math.Round((users.Count(l => l.Contains("Advanced", StringComparison.OrdinalIgnoreCase)) / (double)total) * 100, 1) }
            };
        }

        private async Task<List<object>> GetTopCoursesAsync()
        {
            // Get all courses that are not deleted
            var allCourses = await _context.Courses
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.Title)
                .ToListAsync();

            // Get enrollment counts for each course - count distinct users per course
            var enrollmentCounts = await _context.UserCourses
                .Where(uc => !uc.isDeleted)
                .GroupBy(uc => new { uc.CourseId, uc.UserId })
                .Select(g => g.Key.CourseId)
                .GroupBy(courseId => courseId)
                .Select(g => new
                {
                    CourseId = g.Key,
                    EnrollmentCount = g.Count()
                })
                .ToDictionaryAsync(x => x.CourseId, x => x.EnrollmentCount);

            var result = new List<object>();
            foreach (var course in allCourses)
            {
                var enrollmentCount = enrollmentCounts.TryGetValue(course.Id, out var count) ? count : 0;
                
                result.Add(new
                {
                    CourseId = course.Id,
                    Title = course.Title ?? "Untitled Course",
                    EnrollmentCount = enrollmentCount
                });
            }

            return result;
        }

        // ============================================
        // USER MANAGEMENT
        // ============================================
        public async Task<IActionResult> UserManagment(string searchTerm = "", int page = 1, int pageSize = 10)
        {
            var query = _context.Users
                .AsNoTracking()
                .Include(u => u.LessonCompletions)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(u => 
                    u.FirstName.Contains(searchTerm) || 
                    u.LastName.Contains(searchTerm) || 
                    u.Email.Contains(searchTerm));
            }

            var totalUsers = await query.CountAsync();
            var users = await query
                .OrderByDescending(u => u.Id) // Order by Id since CreatedAt doesn't exist
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Calculate total lessons for progress calculation
            var totalLessons = await _context.Lessons.CountAsync();

            ViewBag.Users = users;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalLessons = totalLessons;

            return View();
        }

        public async Task<IActionResult> UserDetails(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.UserCourses)
                    .ThenInclude(uc => uc.Course)
                        .ThenInclude(c => c.Lessons)
                .Include(u => u.LessonCompletions)
                    .ThenInclude(lc => lc.Lesson)
                .Include(u => u.ExerciseSubmissions)
                .Include(u => u.UserQuizAttempts)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            // Calculate user statistics
            var completedLessons = user.LessonCompletions.Count;
            var completedExercises = user.ExerciseSubmissions.Count(e => e.IsCorrect);
            var quizAttempts = user.UserQuizAttempts.Count;
            var passedQuizzes = user.UserQuizAttempts.Count(q => q.IsPassed);

            ViewBag.User = user;
            ViewBag.CompletedLessons = completedLessons;
            ViewBag.CompletedExercises = completedExercises;
            ViewBag.QuizAttempts = quizAttempts;
            ViewBag.PassedQuizzes = passedQuizzes;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ToggleAdminStatus(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound();
            }

            user.IsAdmin = !user.IsAdmin;
            await _userManager.UpdateAsync(user);

            // Update claim if needed
            var claims = await _userManager.GetClaimsAsync(user);
            var adminClaim = claims.FirstOrDefault(c => c.Type == "IsAdmin");
            
            if (user.IsAdmin && adminClaim == null)
            {
                await _userManager.AddClaimAsync(user, new Claim("IsAdmin", "true"));
            }
            else if (!user.IsAdmin && adminClaim != null)
            {
                await _userManager.RemoveClaimAsync(user, adminClaim);
            }

            TempData["Success"] = $"User admin status updated to {(user.IsAdmin ? "Admin" : "Regular User")}";
            return RedirectToAction(nameof(UserDetails), new { id = userId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction(nameof(UserManagment));
            }

            try
            {
                // Delete UserAnswers before UserAssessments (Restrict FK)
                var assessmentIds = _context.UserAssessments
                    .Where(ua => ua.UserId == userId)
                    .Select(ua => ua.Id);
                _context.UserAnswers.RemoveRange(
                    _context.UserAnswers.Where(ua => assessmentIds.Contains(ua.UserAssessmentId)));
                _context.UserAssessments.RemoveRange(
                    _context.UserAssessments.Where(ua => ua.UserId == userId));

                // Delete remaining related data
                _context.ExerciseSubmissions.RemoveRange(
                    _context.ExerciseSubmissions.Where(es => es.UserId == userId));
                _context.LessonCompletions.RemoveRange(
                    _context.LessonCompletions.Where(lc => lc.UserId == userId));
                _context.UserCourses.RemoveRange(
                    _context.UserCourses.Where(uc => uc.UserId == userId));
                _context.Projects.RemoveRange(
                    _context.Projects.Where(p => p.UserId == userId));

                var cv = await _context.CVs.FirstOrDefaultAsync(c => c.UserId == userId);
                if (cv != null) _context.CVs.Remove(cv);

                await _context.SaveChangesAsync();

                // Identity handles AspNetUserLogins, AspNetUserClaims, AspNetUserRoles automatically
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    TempData["Success"] = "User deleted successfully.";
                else
                    TempData["Error"] = "Failed to delete user: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting user: {ex.Message}";
            }

            return RedirectToAction(nameof(UserManagment));
        }

        // ============================================
        // COURSE MANAGEMENT
        // ============================================
        public async Task<IActionResult> CourseManagment(string searchTerm = "", int page = 1, int pageSize = 10)
        {
            var query = _context.Courses.AsNoTracking().Where(c => !c.IsDeleted).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c =>
                    c.Title.Contains(searchTerm) ||
                    c.Description.Contains(searchTerm) ||
                    c.LearningPath.Contains(searchTerm));
            }

            var totalCourses = await query.CountAsync();
            var courses = await query
                .Include(c => c.Lessons)
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Courses = courses;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCourses / (double)pageSize);
            ViewBag.TotalCourses = totalCourses;

            return View();
        }

        public async Task<IActionResult> CRUDCourse(Guid? id)
        {
            CourseWithLessonsViewModel viewModel;
            if (id.HasValue)
            {
                var course = await _context.Courses
                    .Include(c => c.Lessons.Where(l => !l.isDeleted))
                    .FirstOrDefaultAsync(c => c.Id == id.Value && !c.IsDeleted);
                
                if (course == null)
                {
                    return NotFound();
                }

                viewModel = new CourseWithLessonsViewModel
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                    DifficultyLevel = course.DifficultyLevel,
                    LearningPath = course.LearningPath,
                    ProgrammingLanguage = course.ProgrammingLanguage,
                    Lessons = course.Lessons
                        .OrderBy(l => l.OrderNumber)
                        .Select(l => new LessonViewModel
                        {
                            Id = l.Id,
                            CourseId = l.CourseId,
                            Title = l.Title,
                            Content = l.Content,
                            VideoUrl = l.VideoUrl,
                            ImageUrl = l.ImageUrl,
                            OrderNumber = l.OrderNumber,
                            IsDeleted = l.isDeleted
                        }).ToList()
                };
                ViewBag.IsEdit = true;
            }
            else
            {
                viewModel = new CourseWithLessonsViewModel
                {
                    Id = Guid.Empty,
                    Lessons = new List<LessonViewModel>()
                };
                ViewBag.IsEdit = false;
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrUpdateCourse(CourseWithLessonsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Course course;
                if (viewModel.Id == Guid.Empty)
                {
                    // Create new course
                    course = new Course
                    {
                        Id = Guid.NewGuid(),
                        Title = viewModel.Title,
                        Description = viewModel.Description,
                        DifficultyLevel = viewModel.DifficultyLevel,
                        LearningPath = viewModel.LearningPath,
                        ProgrammingLanguage = viewModel.ProgrammingLanguage,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    };
                    _context.Courses.Add(course);
                    await _context.SaveChangesAsync(); // Save to get the course ID
                    TempData["Success"] = "Course created successfully";
                }
                else
                {
                    // Update existing course
                    course = await _context.Courses.FindAsync(viewModel.Id);
                    if (course == null)
                    {
                        return NotFound();
                    }

                    course.Title = viewModel.Title;
                    course.Description = viewModel.Description;
                    course.DifficultyLevel = viewModel.DifficultyLevel;
                    course.LearningPath = viewModel.LearningPath;
                    course.ProgrammingLanguage = viewModel.ProgrammingLanguage;
                    _context.Courses.Update(course);
                    TempData["Success"] = "Course updated successfully";
                }

                // Handle lessons
                if (viewModel.Lessons != null && viewModel.Lessons.Any())
                {
                    var existingLessons = await _context.Lessons
                        .Where(l => l.CourseId == course.Id)
                        .ToListAsync();

                    foreach (var lessonVm in viewModel.Lessons)
                    {
                        if (lessonVm.IsDeleted)
                        {
                            // Mark for deletion (soft delete)
                            var lessonToDelete = existingLessons.FirstOrDefault(l => l.Id == lessonVm.Id);
                            if (lessonToDelete != null)
                            {
                                lessonToDelete.isDeleted = true;
                            }
                        }
                        else if (lessonVm.Id == Guid.Empty)
                        {
                            // New lesson
                            var newLesson = new Lesson
                            {
                                Id = Guid.NewGuid(),
                                CourseId = course.Id,
                                Title = lessonVm.Title,
                                Content = lessonVm.Content,
                                VideoUrl = lessonVm.VideoUrl,
                                ImageUrl = lessonVm.ImageUrl,
                                OrderNumber = lessonVm.OrderNumber,
                                CreatedAt = DateTime.UtcNow,
                                isDeleted = false
                            };
                            _context.Lessons.Add(newLesson);
                        }
                        else
                        {
                            // Update existing lesson
                            var existingLesson = existingLessons.FirstOrDefault(l => l.Id == lessonVm.Id);
                            if (existingLesson != null)
                            {
                                existingLesson.Title = lessonVm.Title;
                                existingLesson.Content = lessonVm.Content;
                                existingLesson.VideoUrl = lessonVm.VideoUrl;
                                existingLesson.ImageUrl = lessonVm.ImageUrl;
                                existingLesson.OrderNumber = lessonVm.OrderNumber;
                                existingLesson.isDeleted = false;
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CourseManagment));
            }

            ViewBag.IsEdit = viewModel.Id != Guid.Empty;
            return View("CRUDCourse", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                course.IsDeleted = true;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Course deleted successfully";
            }

            return RedirectToAction(nameof(CourseManagment));
        }

        // ============================================
        // USER CRUD
        // ============================================
        public async Task<IActionResult> CRUDUser(Guid? id)
        {
            if (id.HasValue)
            {
                var existingUser = await _context.Users.FindAsync(id.Value);
                if (existingUser == null) return NotFound();
                return View("EditUser", existingUser);
            }

            return View(new ApplicationUser());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrUpdateUser(ApplicationUser user, string password, string confirmPassword, bool isAdmin)
        {
            var isEdit = user.Id != Guid.Empty;
            var returnView = isEdit ? "EditUser" : "CRUDUser";

            // Validate manually — ModelState.IsValid always fails for IdentityUser-derived models
            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                TempData["Error"] = "First name is required.";
                return View(returnView, user);
            }
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                TempData["Error"] = "Email is required.";
                return View(returnView, user);
            }

            if (!isEdit)
            {
                // CREATE
                if (string.IsNullOrWhiteSpace(password) || password != confirmPassword)
                {
                    TempData["Error"] = "Password is required and must match the confirmation.";
                    return View("CRUDUser", user);
                }

                user.Id = Guid.NewGuid();
                user.UserName = user.Email;
                user.IsAdmin = isAdmin;
                user.OnboardingCompleted = true;
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    if (isAdmin)
                        await _userManager.AddClaimAsync(user, new Claim("IsAdmin", "true"));

                    TempData["Success"] = "User created successfully.";
                    return RedirectToAction(nameof(UserManagment));
                }

                TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
                return View("CRUDUser", user);
            }
            else
            {
                // EDIT
                var existingUser = await _userManager.FindByIdAsync(user.Id.ToString());
                if (existingUser == null)
                {
                    TempData["Error"] = "User not found.";
                    return RedirectToAction(nameof(UserManagment));
                }

                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;
                existingUser.UserName = user.Email;
                existingUser.Level = user.Level;
                existingUser.LearningPath = user.LearningPath;
                existingUser.IsAdmin = isAdmin;

                var updateResult = await _userManager.UpdateAsync(existingUser);

                if (!updateResult.Succeeded)
                {
                    TempData["Error"] = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                    return View("EditUser", existingUser);
                }

                // Optional password change
                if (!string.IsNullOrWhiteSpace(password))
                {
                    if (password != confirmPassword)
                    {
                        TempData["Error"] = "Passwords do not match. Other changes were saved.";
                        return View("EditUser", existingUser);
                    }
                    var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                    await _userManager.ResetPasswordAsync(existingUser, token, password);
                }

                // Sync IsAdmin claim
                var claims = await _userManager.GetClaimsAsync(existingUser);
                var adminClaim = claims.FirstOrDefault(c => c.Type == "IsAdmin");
                if (isAdmin && adminClaim == null)
                    await _userManager.AddClaimAsync(existingUser, new Claim("IsAdmin", "true"));
                else if (!isAdmin && adminClaim != null)
                    await _userManager.RemoveClaimAsync(existingUser, adminClaim);

                TempData["Success"] = "User updated successfully.";
                return RedirectToAction(nameof(UserManagment));
            }
        }

        // ============================================
        // REPORTS / ANALYTICS
        // ============================================
        public async Task<IActionResult> Reports()
        {
            var totalUsers = await _context.Users.CountAsync();

            var totalCourses = await _context.Courses.Where(c => !c.IsDeleted).CountAsync();
            var totalLessons = await _context.Lessons.CountAsync();
            var totalExercises = await _context.CodingExercises.CountAsync();

            // User progress statistics
            var usersWithProgress = await _context.Users
                .Where(u => u.LessonCompletions.Any() || u.ExerciseSubmissions.Any())
                .Select(u => new
                {
                    UserId = u.Id,
                    CompletedLessons = u.LessonCompletions.Count,
                    CompletedExercises = u.ExerciseSubmissions.Count(e => e.IsCorrect),
                    QuizAttempts = u.UserQuizAttempts.Count,
                    PassedQuizzes = u.UserQuizAttempts.Count(q => q.IsPassed)
                })
                .ToListAsync();

            // Same filter as usersWithProgress above — reuse the count instead of re-querying.
            var activeUsers = usersWithProgress.Count;

            // Course popularity
            var rawEnrollments = await _context.UserCourses
                .GroupBy(uc => uc.CourseId)
                .Select(g => new { CourseId = g.Key, EnrollmentCount = g.Count() })
                .ToListAsync();
            var enrolledCourseIds = rawEnrollments.Select(e => e.CourseId).ToList();
            var courseTitleMap = await _context.Courses
                .Where(c => enrolledCourseIds.Contains(c.Id))
                .Select(c => new { c.Id, c.Title })
                .ToDictionaryAsync(c => c.Id, c => c.Title ?? "Untitled Course");
            var courseEnrollments = rawEnrollments
                .Select(e => new
                {
                    e.CourseId,
                    e.EnrollmentCount,
                    CourseTitle = courseTitleMap.TryGetValue(e.CourseId, out var t) ? t : "Untitled Course"
                })
                .ToList<object>();

            // User-submitted reports
            var userReports = await _context.UserReports
                .Where(r => !r.IsDeleted)
                .OrderByDescending(r => r.SubmittedAt)
                .ToListAsync();

            // Real job applications (self-reported by users)
            var externalJobApplications = await _context.ExternalJobApplications
                .Include(e => e.User)
                .OrderByDescending(e => e.AppliedDate)
                .Select(e => new
                {
                    e.Id,
                    UserName = (e.User.FirstName ?? "") + " " + (e.User.LastName ?? ""),
                    UserEmail = e.User.Email ?? "",
                    e.JobTitle,
                    e.EmployerName,
                    e.JobLocation,
                    e.MatchPercentage,
                    e.AppliedDate,
                    e.ApplyLink
                })
                .ToListAsync();

            ViewBag.TotalUsers = totalUsers;
            ViewBag.ActiveUsers = activeUsers;
            ViewBag.TotalCourses = totalCourses;
            ViewBag.TotalLessons = totalLessons;
            ViewBag.TotalExercises = totalExercises;
            ViewBag.UsersWithProgress = usersWithProgress.Cast<object>().ToList();
            ViewBag.CourseEnrollments = courseEnrollments;
            ViewBag.UserReports = userReports;
            ViewBag.ExternalJobApplications = externalJobApplications.Cast<object>().ToList();
            ViewBag.TotalExternalApplications = externalJobApplications.Count;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateReportStatus(Guid reportId, string status)
        {
            var report = await _context.UserReports.FindAsync(reportId);
            if (report != null)
            {
                report.Status = status;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Reports");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReport(Guid reportId)
        {
            var report = await _context.UserReports.FindAsync(reportId);
            if (report != null)
            {
                report.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Reports");
        }

        // ============================================
        // EXPORT
        // ============================================
        public async Task<IActionResult> ExportUsers()
        {
            var users = await _context.Users
                .OrderByDescending(u => u.Id)
                .Select(u => new
                {
                    FirstName    = u.FirstName    ?? "",
                    LastName     = u.LastName     ?? "",
                    Email        = u.Email        ?? "",
                    LearningPath = u.LearningPath ?? "None",
                    Level        = u.Level        ?? "New",
                    u.IsAdmin
                })
                .ToListAsync();

            var csv = new System.Text.StringBuilder();
            csv.AppendLine("Name,Email,Learning Path,Level,Is Admin");
            foreach (var u in users)
            {
                var name = (u.FirstName + " " + u.LastName).Trim();
                csv.AppendLine($"\"{name}\",\"{u.Email}\",\"{u.LearningPath}\",\"{u.Level}\",{u.IsAdmin}");
            }

            return File(
                System.Text.Encoding.UTF8.GetBytes(csv.ToString()),
                "text/csv",
                "codewave-users.csv");
        }

        // ============================================
        // HELPER METHODS
        // ============================================
        private async Task<List<object>> GetRecentActivityAsync()
        {
            var activities = new List<object>();

            // Recent user registrations
            var recentUsers = await _context.Users
                .OrderByDescending(u => u.Id) // Order by Id since CreatedAt doesn't exist
                .Take(5)
                .Select(u => new { Type = "User Registration", User = u.Email, Date = DateTime.UtcNow }) // Use current date as placeholder
                .ToListAsync();

            // Recent course completions
            var recentCompletions = await _context.LessonCompletions
                .Include(lc => lc.Lesson)
                .Include(lc => lc.User)
                .Where(lc => lc.CompletionDate.HasValue)
                .OrderByDescending(lc => lc.CompletionDate)
                .Take(5)
                .Select(lc => new { Type = "Lesson Completed", User = lc.User.Email, Lesson = lc.Lesson.Title, Date = lc.CompletionDate.Value })
                .ToListAsync();

            activities.AddRange(recentUsers);
            activities.AddRange(recentCompletions);

            return activities.OrderByDescending(a => ((dynamic)a).Date).Take(10).ToList<object>();
        }
    }
}

