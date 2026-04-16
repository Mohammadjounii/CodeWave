using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Repositories;

public class QuizRepository : IQuizRepository
{
    private readonly ApplicationDbContext _context;

    public QuizRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Quiz>> GetAllQuizzesAsync()
    {
        return await _context.Quizzes
            .Include(q => q.Questions)
                .ThenInclude(qq => qq.AnswerOptions)
            .Where(q => !q.IsDeleted)
            .OrderBy(q => q.CreatedAt)
            .ToListAsync();
    }

    public async Task<Quiz?> GetQuizByIdAsync(Guid id)
    {
        return await _context.Quizzes
            .Include(q => q.Questions)
                .ThenInclude(qq => qq.AnswerOptions)
            .FirstOrDefaultAsync(q => q.Id == id && !q.IsDeleted);
    }
}

