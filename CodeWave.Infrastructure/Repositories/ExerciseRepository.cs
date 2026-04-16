using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Repositories;

public class ExerciseRepository : IExerciseRepository
{
    private readonly ApplicationDbContext _context;

    public ExerciseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<CodingExercise?> GetByIdAsync(Guid exerciseId)
    {
        return _context.CodingExercises
            .FirstOrDefaultAsync(e => e.Id == exerciseId && !e.isDeleted);
    }

    public Task<CodingExercise?> GetWithLessonAsync(Guid exerciseId)
    {
        return _context.CodingExercises
            .Include(e => e.Lesson)
            .Include(e => e.TestCases.Where(tc => !tc.IsDeleted))
            .FirstOrDefaultAsync(e => e.Id == exerciseId && !e.isDeleted);
    }
}

