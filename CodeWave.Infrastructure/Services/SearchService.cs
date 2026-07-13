using CodeWave.Application.DTOs;
using CodeWave.Application.Interfaces;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace CodeWave.Infrastructure.Services
{
    public class SearchService : ISearchService
    {
        private readonly ApplicationDbContext _context;

        public SearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SearchResultDto>> SearchAsync(string query, Guid? userId = null)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Trim().Length < 2)
                return new List<SearchResultDto>();

            var q = query.Trim().ToLower();
            var results = new List<SearchResultDto>();

            // ── Courses ──────────────────────────────────────────────────────────
            var courses = await _context.Courses
                .Where(c => !c.IsDeleted &&
                    (c.Title.ToLower().Contains(q) || c.Description.ToLower().Contains(q)))
                .Take(8).ToListAsync();

            results.AddRange(courses.Select(c => new SearchResultDto
            {
                Id            = c.Id,
                Type          = "Course",
                Title         = c.Title,
                Excerpt       = Truncate(c.Description, 130),
                LearningPath  = c.LearningPath,
                DifficultyLevel = c.DifficultyLevel,
                Url           = $"/LearningPath/Course?courseId={c.Id}",
                TypeIcon      = "school",
                TypeColor     = "purple"
            }));

            // ── Lessons ───────────────────────────────────────────────────────────
            var lessons = await _context.Lessons
                .Include(l => l.Course)
                .Where(l => !l.isDeleted &&
                    (l.Course == null || !l.Course.IsDeleted) &&
                    l.Title.ToLower().Contains(q))
                .Take(8).ToListAsync();

            results.AddRange(lessons.Select(l => new SearchResultDto
            {
                Id           = l.Id,
                Type         = "Lesson",
                Title        = l.Title,
                Excerpt      = Truncate(StripHtml(l.Content), 130),
                LearningPath = l.Course?.LearningPath,
                Url          = $"/LearningPath/Course?courseId={l.CourseId}",
                TypeIcon     = "menu_book",
                TypeColor    = "blue"
            }));

            // ── Quizzes ───────────────────────────────────────────────────────────
            var quizzes = await _context.Quizzes
                .Include(x => x.Course)
                .Where(x => !x.IsDeleted &&
                    (x.Course == null || !x.Course.IsDeleted) &&
                    (x.Title.ToLower().Contains(q) || x.Description.ToLower().Contains(q)))
                .Take(6).ToListAsync();

            results.AddRange(quizzes.Select(x => new SearchResultDto
            {
                Id           = x.Id,
                Type         = "Quiz",
                Title        = x.Title,
                Excerpt      = Truncate(x.Description, 130),
                LearningPath = x.Course?.LearningPath,
                Url          = $"/Quiz/Take?quizId={x.Id}",
                TypeIcon     = "quiz",
                TypeColor    = "yellow"
            }));

            // ── Achievements ──────────────────────────────────────────────────────
            var achievements = await _context.Achievements
                .Where(a => a.Name.ToLower().Contains(q) ||
                            a.Description.ToLower().Contains(q) ||
                            a.Category.ToLower().Contains(q))
                .Take(6).ToListAsync();

            results.AddRange(achievements.Select(a => new SearchResultDto
            {
                Id           = a.Id,
                Type         = "Achievement",
                Title        = a.Name,
                Excerpt      = Truncate(a.Description, 130),
                LearningPath = a.Category,
                Url          = "/Achievements",
                TypeIcon     = "military_tech",
                TypeColor    = "orange"
            }));

            // ── Interview Questions ───────────────────────────────────────────────
            var interviews = await _context.InterviewQuestions
                .Where(i => i.Question.ToLower().Contains(q) ||
                            i.Category.ToLower().Contains(q) ||
                            i.Tags.ToLower().Contains(q))
                .Take(6).ToListAsync();

            results.AddRange(interviews.Select(i => new SearchResultDto
            {
                Id              = i.Id,
                Type            = "Interview",
                Title           = Truncate(i.Question, 80),
                Excerpt         = Truncate(i.ModelAnswer, 130),
                LearningPath    = i.Category,
                DifficultyLevel = i.Difficulty,
                Url             = "/Interview",
                TypeIcon        = "record_voice_over",
                TypeColor       = "green"
            }));

            // ── Job Offers ────────────────────────────────────────────────────────
            var jobs = await _context.JobOffers
                .Where(j => !j.isDeleted &&
                    j.Deadline >= DateTime.UtcNow &&
                    (j.JobTitle.ToLower().Contains(q) ||
                     j.Company.ToLower().Contains(q) ||
                     j.Description.ToLower().Contains(q) ||
                     j.RequiredSkills.ToLower().Contains(q)))
                .Take(6).ToListAsync();

            results.AddRange(jobs.Select(j => new SearchResultDto
            {
                Id      = j.Id,
                Type    = "Job",
                Title   = j.JobTitle,
                Excerpt = $"{j.Company} — {Truncate(j.Description, 100)}",
                Url     = $"/JobOffer?selectedJobId={j.Id}",
                TypeIcon  = "work",
                TypeColor = "rose"
            }));

            // ── Coding Exercises ──────────────────────────────────────────────────
            var exercises = await _context.CodingExercises
                .Include(e => e.Lesson)
                    .ThenInclude(l => l.Course)
                .Where(e => !e.isDeleted &&
                    (e.Title.ToLower().Contains(q) ||
                     e.Description.ToLower().Contains(q)))
                .Take(6).ToListAsync();

            results.AddRange(exercises.Select(e => new SearchResultDto
            {
                Id           = e.Id,
                Type         = "Exercise",
                Title        = e.Title,
                Excerpt      = Truncate(e.Description, 130),
                LearningPath = e.Lesson?.Course?.LearningPath,
                Url          = e.Lesson != null
                    ? $"/LearningPath/Course?courseId={e.Lesson.CourseId}"
                    : "/Home",
                TypeIcon  = "code",
                TypeColor = "indigo"
            }));

            // ── User Projects (only if logged in) ─────────────────────────────────
            if (userId.HasValue)
            {
                var projects = await _context.Projects
                    .Where(p => !p.isDeleted &&
                        p.UserId == userId.Value &&
                        (p.Title.ToLower().Contains(q) ||
                         (p.Description != null && p.Description.ToLower().Contains(q))))
                    .Take(4).ToListAsync();

                results.AddRange(projects.Select(p => new SearchResultDto
                {
                    Id      = p.Id,
                    Type    = "Project",
                    Title   = p.Title,
                    Excerpt = Truncate(p.Description, 130),
                    Url     = "/Projects",
                    TypeIcon  = "folder_code",
                    TypeColor = "cyan"
                }));
            }

            // ── Static feature pages ──────────────────────────────────────────
            results.AddRange(MatchFeaturePages(q));

            return results
                .OrderByDescending(r => RelevanceScore(r.Title, q))
                .ThenBy(r => r.Title)
                .ToList();
        }

        private static List<SearchResultDto> MatchFeaturePages(string q)
        {
            var pages = new (string Title, string Excerpt, string Url, string Icon, string Color, string[] Keywords)[]
            {
                ("Settings",
                 "Manage your profile, password, theme and account preferences.",
                 "/Settings", "settings", "purple",
                 new[]{"settings","profile","password","account","theme","dark mode","light mode","preferences"}),

                ("Code Playground",
                 "Write and run Python or Java code directly in your browser.",
                 "/Playground", "terminal", "indigo",
                 new[]{"playground","run code","code editor","compiler","execute","sandbox","terminal","code runner"}),

                ("My CV",
                 "Build, edit and download your professional programming resume.",
                 "/Cv", "description", "cyan",
                 new[]{"cv","resume","curriculum","vitae","download","pdf","certificate"}),
            };

            return pages
                .Where(p => p.Keywords.Any(k => k.Contains(q) || q.Contains(k)))
                .Select(p => new SearchResultDto
                {
                    Id        = Guid.NewGuid(),
                    Type      = "Feature",
                    Title     = p.Title,
                    Excerpt   = p.Excerpt,
                    Url       = p.Url,
                    TypeIcon  = p.Icon,
                    TypeColor = p.Color
                })
                .ToList();
        }

        public async Task<List<SearchResultDto>> SuggestAsync(string query, Guid? userId = null)
        {
            var all = await SearchAsync(query, userId);
            return all.Take(7).ToList();
        }

        private static int RelevanceScore(string title, string q)
        {
            var t = title.ToLower();
            if (t == q)          return 4;
            if (t.StartsWith(q)) return 3;
            if (t.Contains(q))   return 2;
            return 1;
        }

        private static string Truncate(string? text, int max)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            return text.Length <= max ? text : text[..max] + "…";
        }

        private static string StripHtml(string? html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;
            return Regex.Replace(html, "<[^>]+>", " ").Replace("  ", " ").Trim();
        }
    }
}
