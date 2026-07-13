using CodeWave.Domain.Entities;

namespace CodeWave.Application.Interfaces;

public interface IInterviewService
{
    Task<List<InterviewQuestion>>     GetAllQuestionsAsync();
    Task<List<UserInterviewPractice>> GetUserPracticesAsync(Guid userId);
    Task<bool> SavePracticeAsync(Guid userId, Guid questionId, int selfRating, string? notes);
    Task<bool> RemovePracticeAsync(Guid userId, Guid questionId);
}
