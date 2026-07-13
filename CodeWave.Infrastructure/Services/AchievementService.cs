using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Services;

public class AchievementService : IAchievementService
{
    private readonly ApplicationDbContext _db;

    public AchievementService(ApplicationDbContext db) => _db = db;

    public Task<List<Achievement>> GetAllAchievementsAsync() =>
        _db.Achievements.AsNoTracking().OrderBy(a => a.Category).ThenBy(a => a.XPValue).ToListAsync();

    public Task<List<UserAchievement>> GetUserAchievementsAsync(Guid userId) =>
        _db.UserAchievements
            .AsNoTracking()
            .Include(ua => ua.Achievement)
            .Where(ua => ua.UserId == userId)
            .OrderByDescending(ua => ua.EarnedAt)
            .ToListAsync();

    public async Task<bool> TryAwardAsync(Guid userId, string key)
    {
        try
        {
            var achievement = await _db.Achievements.FirstOrDefaultAsync(a => a.Key == key);
            if (achievement == null) return false;

            bool alreadyEarned = await _db.UserAchievements
                .AnyAsync(ua => ua.UserId == userId && ua.AchievementId == achievement.Id);
            if (alreadyEarned) return false;

            _db.UserAchievements.Add(new UserAchievement
            {
                Id            = Guid.NewGuid(),
                UserId        = userId,
                AchievementId = achievement.Id,
                EarnedAt      = DateTime.UtcNow
            });
            await _db.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task CheckLessonAchievementsAsync(Guid userId)
    {
        try
        {
            var completions = await _db.LessonCompletions
                .AsNoTracking()
                .Where(lc => lc.UserId == userId && lc.IsCompleted && !lc.isDeleted)
                .ToListAsync();

            int count = completions.Count;

            if (count >= 1)  await TryAwardAsync(userId, "first_lesson");
            if (count >= 5)  await TryAwardAsync(userId, "lesson_5");
            if (count >= 10) await TryAwardAsync(userId, "lesson_10");
            if (count >= 25) await TryAwardAsync(userId, "lesson_25");

            // 3-day streak: at least one completion on 3 consecutive calendar days
            var distinctDays = completions
                .Where(lc => lc.CompletionDate.HasValue)
                .Select(lc => lc.CompletionDate!.Value.Date)
                .Distinct()
                .OrderByDescending(d => d)
                .ToList();

            if (HasConsecutiveDays(distinctDays, 3))
                await TryAwardAsync(userId, "streak_3");

            // Course completion: check via a pure EF join (no mixed in-memory/DB LINQ)
            var enrolledCourseIds = await _db.UserCourses
                .AsNoTracking()
                .Where(uc => uc.UserId == userId && !uc.isDeleted)
                .Select(uc => uc.CourseId)
                .ToListAsync();

            foreach (var courseId in enrolledCourseIds)
            {
                var totalLessons = await _db.Lessons
                    .CountAsync(l => l.CourseId == courseId && !l.isDeleted);

                if (totalLessons == 0) continue;

                // Pure DB join — no in-memory .Count(x => dbQuery)
                var completedInCourse = await _db.LessonCompletions
                    .Where(lc => lc.UserId == userId && lc.IsCompleted && !lc.isDeleted)
                    .Join(_db.Lessons.Where(l => l.CourseId == courseId && !l.isDeleted),
                          lc => lc.LessonId,
                          l  => l.Id,
                          (lc, l) => lc.Id)
                    .CountAsync();

                if (completedInCourse >= totalLessons)
                {
                    await TryAwardAsync(userId, "course_complete");
                    break;
                }
            }
        }
        catch { /* never let achievement errors surface to the caller */ }
    }

    public async Task CheckExerciseAchievementsAsync(Guid userId)
    {
        try
        {
            var correctCount = await _db.ExerciseSubmissions
                .AsNoTracking()
                .Where(es => es.UserId == userId && es.IsCorrect)
                .Select(es => es.ExerciseId)
                .Distinct()
                .CountAsync();

            if (correctCount >= 1)  await TryAwardAsync(userId, "first_exercise");
            if (correctCount >= 10) await TryAwardAsync(userId, "exercise_10");
        }
        catch { }
    }

    public async Task CheckQuizAchievementsAsync(Guid userId, double score, bool isPassed)
    {
        try
        {
            if (isPassed)
            {
                await TryAwardAsync(userId, "first_quiz");

                var passedCount = await _db.UserQuizAttempts
                    .AsNoTracking()
                    .CountAsync(a => a.UserId == userId && a.IsPassed);
                if (passedCount >= 3) await TryAwardAsync(userId, "quiz_3");
            }

            if (score >= 100.0) await TryAwardAsync(userId, "quiz_master");
        }
        catch { }
    }

    // Returns true if the list of distinct days contains at least `needed` consecutive days
    private static bool HasConsecutiveDays(List<DateTime> days, int needed)
    {
        if (days.Count < needed) return false;
        int streak = 1;
        for (int i = 0; i < days.Count - 1; i++)
        {
            if ((days[i] - days[i + 1]).Days == 1)
            {
                if (++streak >= needed) return true;
            }
            else
            {
                streak = 1;
            }
        }
        return streak >= needed;
    }
}
