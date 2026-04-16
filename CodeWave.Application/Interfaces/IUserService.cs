using CodeWave.Domain.Entities;

namespace CodeWave.Application.Interfaces;

public interface IUserService
{
    Task<Course?> GetRecommendedCourseByLearningPathAsync(string learningPath);
    Task<List<UserCourse>> GetUserCoursesAsync(Guid userId);
    Task<int> GetCompletedCoursesCountAsync(Guid userId);
    Task<int> GetAverageProgressPercentAsync(Guid userId);
}

