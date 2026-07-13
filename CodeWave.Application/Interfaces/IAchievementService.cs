using CodeWave.Domain.Entities;

namespace CodeWave.Application.Interfaces;

public interface IAchievementService
{
    Task<List<Achievement>> GetAllAchievementsAsync();
    Task<List<UserAchievement>> GetUserAchievementsAsync(Guid userId);
    /// <summary>Awards the achievement with the given key if the user hasn't already earned it.</summary>
    Task<bool> TryAwardAsync(Guid userId, string key);
    /// <summary>Checks and awards all lesson-count and streak-based achievements.</summary>
    Task CheckLessonAchievementsAsync(Guid userId);
    /// <summary>Checks and awards exercise-count achievements.</summary>
    Task CheckExerciseAchievementsAsync(Guid userId);
    /// <summary>Checks and awards quiz-based achievements after a quiz is submitted.</summary>
    Task CheckQuizAchievementsAsync(Guid userId, double score, bool isPassed);
}
