using CodeWave.Domain.Entities;

namespace CodeWave.Application.Interfaces;

public interface IExerciseRepository
{
    Task<CodingExercise?> GetByIdAsync(Guid exerciseId);
    Task<CodingExercise?> GetWithLessonAsync(Guid exerciseId);
}

