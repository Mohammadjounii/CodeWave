using CodeWave.Domain.Entities;

namespace CodeWave.Application.Interfaces;

public interface IExerciseSubmissionRepository
{
    Task<List<ExerciseSubmission>> GetLatestForUserAsync(Guid userId, IEnumerable<Guid> exerciseIds);
    Task<ExerciseSubmission?> GetAsync(Guid userId, Guid exerciseId);
    Task AddAsync(ExerciseSubmission submission);
}

