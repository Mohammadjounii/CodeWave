using CodeWave.Domain.Entities;

namespace CodeWave.Application.Interfaces;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(Guid id);
}

