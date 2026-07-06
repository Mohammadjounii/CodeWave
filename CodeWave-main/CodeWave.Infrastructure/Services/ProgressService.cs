using CodeWave.Application.DTOs;
using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeWave.Infrastructure.Services
{
    public class ProgressService : IProgressService
    {
        private readonly ApplicationDbContext _context;

        public ProgressService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserProgressDto> GetUserProgressAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.LessonCompletions)
                .Include(u => u.ExerciseSubmissions)
                .Include(u => u.UserQuizAttempts)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return new UserProgressDto();

            var learningPath = user.LearningPath ?? "Python";

            // Get courses for this learning path (don't require UserCourse enrollment)
            // This allows progress tracking even if user hasn't been explicitly enrolled
            var learningPathLower = learningPath.ToLower().Trim();
            var courseIds = await _context.Courses
                .Where(c => !c.IsDeleted && (
                    c.LearningPath.ToLower().Contains(learningPathLower) || 
                    learningPathLower.Contains(c.LearningPath.ToLower()) ||
                    (learningPathLower.Contains("python") && c.LearningPath.ToLower().Contains("python")) ||
                    (learningPathLower.Contains("java") && (c.LearningPath.ToLower().Contains("java") || c.LearningPath.ToLower().Contains("java development")))
                ))
                .Select(c => c.Id)
                .ToListAsync();

            // Count lessons
            var totalLessons = await _context.Lessons
                .Where(l => courseIds.Contains(l.CourseId) && !l.isDeleted)
                .CountAsync();

            var completedLessons = await _context.LessonCompletions
                .Include(lc => lc.Lesson)
                .Where(lc => lc.UserId == userId && lc.IsCompleted && !lc.isDeleted &&
                             courseIds.Contains(lc.Lesson.CourseId))
                .CountAsync();

            // Count exercises
            var totalExercises = await _context.CodingExercises
                .Include(e => e.Lesson)
                .Where(e => courseIds.Contains(e.Lesson.CourseId) && !e.isDeleted)
                .CountAsync();

            var completedExercises = await _context.ExerciseSubmissions
                .Include(es => es.Exercise)
                    .ThenInclude(e => e.Lesson)
                .Where(es => es.UserId == userId && es.IsCorrect && !es.isDeleted &&
                            courseIds.Contains(es.Exercise.Lesson.CourseId))
                .CountAsync();

            // Count quizzes
            var totalQuizzes = await _context.Quizzes
                .Where(q => courseIds.Contains(q.CourseId) && !q.IsDeleted)
                .CountAsync();

            var passedQuizzes = await _context.UserQuizAttempts
                .Include(uqa => uqa.Quiz)
                .Where(uqa => uqa.UserId == userId && uqa.IsPassed && !uqa.IsDeleted &&
                             courseIds.Contains(uqa.Quiz.CourseId))
                .CountAsync();

            // Calculate overall progress
            var totalItems = totalLessons + totalExercises + totalQuizzes;
            var completedItems = completedLessons + completedExercises + passedQuizzes;
            var overallProgress = totalItems > 0 ? (double)completedItems / totalItems * 100 : 0;

            // Get total study time
            var totalStudyTime = await GetTotalStudyTimeMinutesAsync(userId, learningPath);

            return new UserProgressDto
            {
                TotalLessons = totalLessons,
                CompletedLessons = completedLessons,
                TotalExercises = totalExercises,
                CompletedExercises = completedExercises,
                TotalQuizzes = totalQuizzes,
                PassedQuizzes = passedQuizzes,
                OverallProgressPercent = Math.Round(overallProgress, 1),
                TotalStudyTimeMinutes = totalStudyTime,
                LearningPath = learningPath
            };
        }

        public async Task<List<SkillProgressDto>> GetUserSkillsAsync(Guid userId, string learningPath)
        {
            // Get courses for this learning path (don't require UserCourse enrollment)
            var learningPathLower = learningPath.ToLower().Trim();
            var courseIds = await _context.Courses
                .Where(c => !c.IsDeleted && (
                    c.LearningPath.ToLower().Contains(learningPathLower) || 
                    learningPathLower.Contains(c.LearningPath.ToLower()) ||
                    (learningPathLower.Contains("python") && c.LearningPath.ToLower().Contains("python")) ||
                    (learningPathLower.Contains("java") && (c.LearningPath.ToLower().Contains("java") || c.LearningPath.ToLower().Contains("java development")))
                ))
                .Select(c => c.Id)
                .ToListAsync();

            // Define skills based on learning path
            var skills = learningPath.ToLower() switch
            {
                "python" => new[] { "Variables & Data Types", "Control Flow", "Functions", "OOP", "Data Structures", "File I/O", "Error Handling" },
                "java" => new[] { "Java Basics", "OOP", "Collections", "Exception Handling", "Multithreading", "File I/O", "Algorithms" },
                _ => new[] { "Fundamentals", "Intermediate Concepts", "Advanced Topics" }
            };

            var skillProgress = new List<SkillProgressDto>();

            foreach (var skill in skills)
            {
                // Get lessons related to this skill - improved matching
                var skillKeywords = skill.ToLower().Split(new[] { ' ', '&', '/' }, StringSplitOptions.RemoveEmptyEntries);
                var skillLessons = await _context.Lessons
                    .Where(l => courseIds.Contains(l.CourseId) && !l.isDeleted &&
                               (skillKeywords.Any(keyword => l.Title.ToLower().Contains(keyword)) ||
                                skillKeywords.Any(keyword => l.Content.ToLower().Contains(keyword))))
                    .Select(l => l.Id)
                    .ToListAsync();

                var completedSkillLessons = await _context.LessonCompletions
                    .Where(lc => lc.UserId == userId && lc.IsCompleted && !lc.isDeleted &&
                                skillLessons.Contains(lc.LessonId))
                    .CountAsync();

                var skillExercises = await _context.CodingExercises
                    .Where(e => skillLessons.Contains(e.LessonId) && !e.isDeleted)
                    .CountAsync();

                var completedSkillExercises = await _context.ExerciseSubmissions
                    .Where(es => es.UserId == userId && es.IsCorrect && !es.isDeleted &&
                               skillLessons.Contains(es.Exercise.LessonId))
                    .CountAsync();

                var totalItems = skillLessons.Count + skillExercises;
                var completedItems = completedSkillLessons + completedSkillExercises;
                var masteryLevel = totalItems > 0 ? (int)((double)completedItems / totalItems * 100) : 0;

                skillProgress.Add(new SkillProgressDto
                {
                    SkillName = skill,
                    MasteryLevel = masteryLevel,
                    LessonsCompleted = completedSkillLessons,
                    ExercisesCompleted = completedSkillExercises,
                    IsMastered = masteryLevel >= 80
                });
            }

            return skillProgress;
        }

        public async Task<List<WeaknessDto>> GetUserWeaknessesAsync(Guid userId, string learningPath)
        {
            // Get courses for this learning path (don't require UserCourse enrollment)
            var learningPathLower = learningPath.ToLower().Trim();
            var courseIds = await _context.Courses
                .Where(c => !c.IsDeleted && (
                    c.LearningPath.ToLower().Contains(learningPathLower) || 
                    learningPathLower.Contains(c.LearningPath.ToLower()) ||
                    (learningPathLower.Contains("python") && c.LearningPath.ToLower().Contains("python")) ||
                    (learningPathLower.Contains("java") && (c.LearningPath.ToLower().Contains("java") || c.LearningPath.ToLower().Contains("java development")))
                ))
                .Select(c => c.Id)
                .ToListAsync();

            var weaknesses = new List<WeaknessDto>();

            // Find topics with low quiz scores
            var quizAttempts = await _context.UserQuizAttempts
                .Include(uqa => uqa.Quiz)
                .Where(uqa => uqa.UserId == userId && !uqa.IsDeleted &&
                             courseIds.Contains(uqa.Quiz.CourseId))
                .ToListAsync();

            // Find exercises with multiple failed attempts
            var failedExercises = await _context.ExerciseSubmissions
                .Include(es => es.Exercise)
                    .ThenInclude(e => e.Lesson)
                .Where(es => es.UserId == userId && !es.IsCorrect && !es.isDeleted &&
                            courseIds.Contains(es.Exercise.Lesson.CourseId))
                .GroupBy(es => es.Exercise.Lesson.Title)
                .Select(g => new
                {
                    Topic = g.Key,
                    FailedAttempts = g.Count(),
                    AverageScore = 0.0
                })
                .Where(x => x.FailedAttempts >= 2)
                .ToListAsync();

            foreach (var failed in failedExercises)
            {
                weaknesses.Add(new WeaknessDto
                {
                    Topic = failed.Topic,
                    Description = $"Multiple failed attempts on exercises related to {failed.Topic}",
                    FailedAttempts = failed.FailedAttempts,
                    AverageScore = failed.AverageScore,
                    RecommendedLessons = new List<string> { failed.Topic }
                });
            }

            // Find quizzes with low scores
            var lowScoreQuizzes = quizAttempts
                .Where(uqa => uqa.Score < 70)
                .GroupBy(uqa => uqa.Quiz.Title)
                .Select(g => new WeaknessDto
                {
                    Topic = g.Key,
                    Description = $"Low performance on {g.Key} quiz",
                    FailedAttempts = g.Count(),
                    AverageScore = g.Average(x => x.Score),
                    RecommendedLessons = new List<string> { g.Key }
                })
                .ToList();

            weaknesses.AddRange(lowScoreQuizzes);

            return weaknesses.OrderByDescending(w => w.FailedAttempts).Take(5).ToList();
        }

        public async Task<int> GetTotalStudyTimeMinutesAsync(Guid userId, string learningPath)
        {
            // Get courses for this learning path (don't require UserCourse enrollment)
            var learningPathLower = learningPath.ToLower().Trim();
            var courseIds = await _context.Courses
                .Where(c => !c.IsDeleted && (
                    c.LearningPath.ToLower().Contains(learningPathLower) || 
                    learningPathLower.Contains(c.LearningPath.ToLower()) ||
                    (learningPathLower.Contains("python") && c.LearningPath.ToLower().Contains("python")) ||
                    (learningPathLower.Contains("java") && (c.LearningPath.ToLower().Contains("java") || c.LearningPath.ToLower().Contains("java development")))
                ))
                .Select(c => c.Id)
                .ToListAsync();

            var lessonIds = await _context.Lessons
                .Where(l => courseIds.Contains(l.CourseId) && !l.isDeleted)
                .Select(l => l.Id)
                .ToListAsync();

            var totalTime = await _context.LessonCompletions
                .Where(lc => lc.UserId == userId && !lc.isDeleted && lessonIds.Contains(lc.LessonId))
                .SumAsync(lc => (int?)lc.TimeSpentMinutes) ?? 0;

            return totalTime;
        }

        public async Task<Dictionary<string, int>> GetStudyTimeByTopicAsync(Guid userId, string learningPath)
        {
            // Get courses for this learning path (don't require UserCourse enrollment)
            var learningPathLower = learningPath.ToLower().Trim();
            var courseIds = await _context.Courses
                .Where(c => !c.IsDeleted && (
                    c.LearningPath.ToLower().Contains(learningPathLower) || 
                    learningPathLower.Contains(c.LearningPath.ToLower()) ||
                    (learningPathLower.Contains("python") && c.LearningPath.ToLower().Contains("python")) ||
                    (learningPathLower.Contains("java") && (c.LearningPath.ToLower().Contains("java") || c.LearningPath.ToLower().Contains("java development")))
                ))
                .Select(c => c.Id)
                .ToListAsync();

            var studyTimeByTopic = await _context.LessonCompletions
                .Include(lc => lc.Lesson)
                .Where(lc => lc.UserId == userId && !lc.isDeleted &&
                            courseIds.Contains(lc.Lesson.CourseId))
                .GroupBy(lc => lc.Lesson.Title)
                .Select(g => new { Topic = g.Key, Time = g.Sum(lc => lc.TimeSpentMinutes) })
                .ToDictionaryAsync(x => x.Topic, x => x.Time);

            return studyTimeByTopic;
        }
    }
}

