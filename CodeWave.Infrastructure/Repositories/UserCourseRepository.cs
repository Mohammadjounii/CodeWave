using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Repositories;

public class UserCourseRepository : IUserCourseRepository
{
    private readonly ApplicationDbContext _context;

    public UserCourseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserCourse>> GetUserCoursesByUserIdAsync(Guid userId)
    {
        return await _context.UserCourses
            .Include(uc => uc.Course)
            .Where(uc => uc.UserId == userId && !uc.isDeleted)
            .ToListAsync();
    }

    public async Task<List<Course>> GetCompletedCoursesByUserIdAsync(Guid userId)
    {
        return await _context.UserCourses
            .Include(uc => uc.Course)
            .Where(uc => uc.UserId == userId && !uc.isDeleted && uc.CompletionDate.HasValue)
            .Select(uc => uc.Course)
            .ToListAsync();
    }

    public async Task<DateTime?> GetCourseCompletionDateAsync(Guid userId, Guid courseId)
    {
        return await _context.UserCourses
            .Where(uc => uc.UserId == userId && uc.CourseId == courseId && uc.CompletionDate.HasValue)
            .Select(uc => uc.CompletionDate!.Value)
            .FirstOrDefaultAsync();
    }
}

