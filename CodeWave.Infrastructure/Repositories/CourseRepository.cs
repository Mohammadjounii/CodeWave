using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Repositories;

public class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _context;

    public CourseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Course?> GetByIdAsync(Guid id)
    {
        return _context.Courses.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
    }
}

