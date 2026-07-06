using CodeWave.Domain.Entities;

namespace CodeWave.Application.Interfaces;

public interface ILessonCompletionRepository
{
    Task<List<Guid>> GetCompletedLessonIdsAsync(Guid userId);
    Task<LessonCompletion?> GetAsync(Guid userId, Guid lessonId);
    Task AddAsync(LessonCompletion completion);
}

