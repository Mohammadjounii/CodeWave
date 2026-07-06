using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Services;

public class InterviewService : IInterviewService
{
    private readonly ApplicationDbContext _db;
    public InterviewService(ApplicationDbContext db) => _db = db;

    public Task<List<InterviewQuestion>> GetAllQuestionsAsync() =>
        _db.InterviewQuestions
           .AsNoTracking()
           .OrderBy(q => q.Category)
           .ThenBy(q => q.OrderNumber)
           .ToListAsync();

    public Task<List<UserInterviewPractice>> GetUserPracticesAsync(Guid userId) =>
        _db.UserInterviewPractices
           .AsNoTracking()
           .Where(p => p.UserId == userId)
           .ToListAsync();

    public async Task<bool> SavePracticeAsync(Guid userId, Guid questionId, int selfRating, string? notes)
    {
        try
        {
            var existing = await _db.UserInterviewPractices
                .FirstOrDefaultAsync(p => p.UserId == userId && p.QuestionId == questionId);

            if (existing != null)
            {
                existing.SelfRating  = selfRating;
                existing.Notes       = notes;
                existing.PracticedAt = DateTime.UtcNow;
                _db.UserInterviewPractices.Update(existing);
            }
            else
            {
                _db.UserInterviewPractices.Add(new UserInterviewPractice
                {
                    Id          = Guid.NewGuid(),
                    UserId      = userId,
                    QuestionId  = questionId,
                    SelfRating  = selfRating,
                    Notes       = notes,
                    PracticedAt = DateTime.UtcNow
                });
            }
            await _db.SaveChangesAsync();
            return true;
        }
        catch { return false; }
    }

    public async Task<bool> RemovePracticeAsync(Guid userId, Guid questionId)
    {
        try
        {
            await _db.UserInterviewPractices
                .Where(p => p.UserId == userId && p.QuestionId == questionId)
                .ExecuteDeleteAsync();
            return true;
        }
        catch { return false; }
    }
}
