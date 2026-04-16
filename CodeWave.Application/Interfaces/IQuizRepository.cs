using CodeWave.Domain.Entities;

namespace CodeWave.Application.Interfaces;

public interface IQuizRepository
{
    Task<List<Quiz>> GetAllQuizzesAsync();
    Task<Quiz?> GetQuizByIdAsync(Guid id);
}

