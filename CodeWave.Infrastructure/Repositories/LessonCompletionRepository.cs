using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Repositories;

public class LessonCompletionRepository : ILessonCompletionRepository
{
    private readonly ApplicationDbContext _context;

    public LessonCompletionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Guid>> GetCompletedLessonIdsAsync(Guid userId)
    {
        return _context.LessonCompletions
            .Where(x => x.UserId == userId && x.IsCompleted && !x.isDeleted)
            .Select(x => x.LessonId)
            .ToListAsync();
    }

    public Task<LessonCompletion?> GetAsync(Guid userId, Guid lessonId)
    {
        return _context.LessonCompletions
            .FirstOrDefaultAsync(c => c.UserId == userId && c.LessonId == lessonId && !c.isDeleted);
    }

    public Task AddAsync(LessonCompletion completion)
    {
        _context.LessonCompletions.Add(completion);
        return Task.CompletedTask;
    }
}

