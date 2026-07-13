using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetUserProjectsAsync(Guid userId)
    {
        return await _context.Projects
            .AsNoTracking()
            .Where(p => p.UserId == userId && !p.isDeleted)
            .OrderByDescending(p => p.CompletionDate)
            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(Guid projectId)
    {
        return await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId && !p.isDeleted);
    }

    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Project project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid projectId)
    {
        await _context.Projects
            .Where(p => p.Id == projectId)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.isDeleted, true));
    }
}
