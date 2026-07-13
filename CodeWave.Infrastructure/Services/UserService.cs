using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Course?> GetRecommendedCourseByLearningPathAsync(string learningPath)
    {
        if (string.IsNullOrEmpty(learningPath))
            return null;

        var learningPathLower = learningPath.ToLower().Trim();
        
        if (learningPathLower.Contains("python") || learningPathLower == "python")
        {
            return await _context.Courses
                .Where(c => c.LearningPath.ToLower().Contains("python") && !c.IsDeleted)
                .OrderBy(c => c.CreatedAt)
                .FirstOrDefaultAsync();
        }
        else if (learningPathLower.Contains("java") || learningPathLower == "java")
        {
            return await _context.Courses
                .Where(c => c.LearningPath.ToLower().Contains("java") && !c.IsDeleted)
                .OrderBy(c => c.CreatedAt)
                .FirstOrDefaultAsync();
        }
        else if (learningPathLower.Contains("web"))
        {
            return await _context.Courses
                .Where(c => c.LearningPath.ToLower().Contains("web") && !c.IsDeleted)
                .OrderBy(c => c.CreatedAt)
                .FirstOrDefaultAsync();
        }

        return null;
    }

    public async Task<List<UserCourse>> GetUserCoursesAsync(Guid userId)
    {
        return await _context.UserCourses
            .Include(uc => uc.Course)
            .Where(uc => uc.UserId == userId && !uc.isDeleted)
            .ToListAsync();
    }

    public async Task<int> GetCompletedCoursesCountAsync(Guid userId)
    {
        return await _context.UserCourses
            .CountAsync(uc => uc.UserId == userId && !uc.isDeleted && uc.CompletionDate.HasValue);
    }

    public async Task<int> GetAverageProgressPercentAsync(Guid userId)
    {
        var userCourses = await GetUserCoursesAsync(userId);
        return userCourses.Any() ? (int)userCourses.Average(uc => uc.ProgressPercent) : 0;
    }
}

