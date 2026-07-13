using CodeWave.Domain.Entities;

namespace CodeWave.Application.Interfaces;

public interface IProjectRepository
{
    Task<List<Project>> GetUserProjectsAsync(Guid userId);
    Task<Project?> GetByIdAsync(Guid projectId);
    Task AddAsync(Project project);
    Task UpdateAsync(Project project);
    Task DeleteAsync(Guid projectId);
}
