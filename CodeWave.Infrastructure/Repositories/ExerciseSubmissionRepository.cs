using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Repositories;

public class ExerciseSubmissionRepository : IExerciseSubmissionRepository
{
    private readonly ApplicationDbContext _context;

    public ExerciseSubmissionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<ExerciseSubmission>> GetLatestForUserAsync(Guid userId, IEnumerable<Guid> exerciseIds)
    {
        return _context.ExerciseSubmissions
            .Where(s => s.UserId == userId && exerciseIds.Contains(s.ExerciseId) && !s.isDeleted)
            .GroupBy(s => s.ExerciseId)
            .Select(g => g.OrderByDescending(x => x.SubmissionDate).First())
            .ToListAsync();
    }

    public Task<ExerciseSubmission?> GetAsync(Guid userId, Guid exerciseId)
    {
        return _context.ExerciseSubmissions
            .FirstOrDefaultAsync(s => s.UserId == userId && s.ExerciseId == exerciseId && !s.isDeleted);
    }

    public Task AddAsync(ExerciseSubmission submission)
    {
        _context.ExerciseSubmissions.Add(submission);
        return Task.CompletedTask;
    }
}

