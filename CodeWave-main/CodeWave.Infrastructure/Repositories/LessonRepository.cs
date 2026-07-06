using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly ApplicationDbContext _context;

    public LessonRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Lesson>> GetByCourseAsync(Guid courseId)
    {
        return _context.Lessons
            .Where(l => l.CourseId == courseId && !l.isDeleted)
            .OrderBy(l => l.OrderNumber)
            .ToListAsync();
    }

    public Task<Lesson?> GetWithExercisesAsync(Guid lessonId)
    {
        return _context.Lessons
            .Include(l => l.CodingExercises.Where(e => !e.isDeleted))
            .FirstOrDefaultAsync(l => l.Id == lessonId && !l.isDeleted);
    }

    public Task<Lesson?> GetByIdAsync(Guid lessonId)
    {
        return _context.Lessons
            .FirstOrDefaultAsync(l => l.Id == lessonId && !l.isDeleted);
    }
}

