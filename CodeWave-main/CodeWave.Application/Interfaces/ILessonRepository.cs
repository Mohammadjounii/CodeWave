using CodeWave.Domain.Entities;

namespace CodeWave.Application.Interfaces;

public interface ILessonRepository
{
    Task<List<Lesson>> GetByCourseAsync(Guid courseId);
    Task<Lesson?> GetWithExercisesAsync(Guid lessonId);
    Task<Lesson?> GetByIdAsync(Guid lessonId);
}

