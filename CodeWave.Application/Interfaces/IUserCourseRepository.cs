using CodeWave.Domain.Entities;

namespace CodeWave.Application.Interfaces;

public interface IUserCourseRepository
{
    Task<List<UserCourse>> GetUserCoursesByUserIdAsync(Guid userId);
    Task<List<Course>> GetCompletedCoursesByUserIdAsync(Guid userId);
    Task<DateTime?> GetCourseCompletionDateAsync(Guid userId, Guid courseId);
}

