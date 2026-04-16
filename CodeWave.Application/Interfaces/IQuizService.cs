using CodeWave.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeWave.Application.Interfaces
{
    public interface IQuizService
    {
        Task<List<QuizDto>> GetQuizzesByCourseAsync(Guid courseId);
        Task<QuizDto> GetQuizByIdAsync(Guid quizId);
        Task<QuizAttemptDto> StartQuizAsync(Guid userId, Guid quizId);
        Task<QuizResultDto> SubmitQuizAsync(Guid attemptId, List<QuizAnswerDto> answers);
        Task<List<QuizAttemptDto>> GetUserQuizAttemptsAsync(Guid userId, Guid? quizId = null);
    }
}

