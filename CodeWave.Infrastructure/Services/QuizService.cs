using CodeWave.Application.DTOs;
using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeWave.Infrastructure.Services
{
    public class QuizService : IQuizService
    {
        private readonly ApplicationDbContext _context;

        public QuizService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<QuizDto>> GetQuizzesByCourseAsync(Guid courseId)
        {
            var quizzes = await _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(qq => qq.AnswerOptions)
                .Where(q => q.CourseId == courseId && !q.IsDeleted)
                .OrderBy(q => q.CreatedAt)
                .ToListAsync();

            return quizzes
                .Where(q => q.Questions.Any(qq => !qq.IsDeleted)) // Only return quizzes with at least one non-deleted question
                .Select(q => new QuizDto
                {
                    Id = q.Id,
                    CourseId = q.CourseId,
                    Title = q.Title,
                    Description = q.Description,
                    TimeLimitMinutes = q.TimeLimitMinutes,
                    PassingScore = q.PassingScore,
                    QuestionCount = q.Questions.Count(qq => !qq.IsDeleted),
                    Questions = q.Questions
                        .Where(qq => !qq.IsDeleted)
                        .OrderBy(qq => qq.OrderNumber)
                        .Select(qq => new QuizQuestionDto
                        {
                            Id = qq.Id,
                            Text = qq.Text,
                            Difficulty = qq.Difficulty,
                            OrderNumber = qq.OrderNumber,
                            AnswerOptions = qq.AnswerOptions
                                .Where(ao => !ao.IsDeleted)
                                .Select(ao => new QuizAnswerOptionDto
                                {
                                    Id = ao.Id,
                                    Text = ao.Text,
                                    IsCorrect = ao.IsCorrect
                                })
                                .OrderBy(ao => Guid.NewGuid()) // Shuffle options
                                .ToList()
                        })
                        .ToList()
                }).ToList();
        }

        public async Task<QuizDto> GetQuizByIdAsync(Guid quizId)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                    .ThenInclude(qq => qq.AnswerOptions)
                .FirstOrDefaultAsync(q => q.Id == quizId && !q.IsDeleted);

            if (quiz == null)
                return null;

            return new QuizDto
            {
                Id = quiz.Id,
                CourseId = quiz.CourseId,
                Title = quiz.Title,
                Description = quiz.Description,
                TimeLimitMinutes = quiz.TimeLimitMinutes,
                PassingScore = quiz.PassingScore,
                QuestionCount = quiz.Questions.Count,
                Questions = quiz.Questions
                    .OrderBy(qq => qq.OrderNumber)
                    .Select(qq => new QuizQuestionDto
                    {
                        Id = qq.Id,
                        Text = qq.Text,
                        Difficulty = qq.Difficulty,
                        OrderNumber = qq.OrderNumber,
                        AnswerOptions = qq.AnswerOptions
                            .Select(ao => new QuizAnswerOptionDto
                            {
                                Id = ao.Id,
                                Text = ao.Text,
                                IsCorrect = ao.IsCorrect
                            })
                            .OrderBy(ao => Guid.NewGuid()) // Shuffle options
                            .ToList()
                    })
                    .ToList()
            };
        }

        public async Task<QuizAttemptDto> StartQuizAsync(Guid userId, Guid quizId)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == quizId && !q.IsDeleted);

            if (quiz == null)
                throw new Exception("Quiz not found");

            // Validate that quiz has at least one question
            var questionCount = quiz.Questions.Count(q => !q.IsDeleted);
            if (questionCount == 0)
            {
                throw new InvalidOperationException($"Quiz '{quiz.Title}' must have at least one question before it can be taken.");
            }

            var attempt = new UserQuizAttempt
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                QuizId = quizId,
                StartedAt = DateTime.UtcNow,
                IsPassed = false,
                Score = 0
            };

            await _context.UserQuizAttempts.AddAsync(attempt);
            await _context.SaveChangesAsync();

            return new QuizAttemptDto
            {
                Id = attempt.Id,
                QuizId = quizId,
                QuizTitle = quiz.Title,
                StartedAt = attempt.StartedAt,
                TimeSpentMinutes = 0
            };
        }

        public async Task<QuizResultDto> SubmitQuizAsync(Guid attemptId, List<QuizAnswerDto> answers)
        {
            var attempt = await _context.UserQuizAttempts
                .Include(uqa => uqa.Quiz)
                    .ThenInclude(q => q.Questions)
                        .ThenInclude(qq => qq.AnswerOptions)
                .FirstOrDefaultAsync(uqa => uqa.Id == attemptId && !uqa.IsDeleted);

            if (attempt == null)
                throw new Exception("Quiz attempt not found");

            if (attempt.CompletedAt.HasValue)
                throw new Exception("Quiz already submitted");

            var totalQuestions = attempt.Quiz.Questions.Count;
            var correctAnswers = 0;
            var questionResults = new List<QuestionResultDto>();

            // Save user answers and check correctness
            foreach (var answer in answers)
            {
                var question = attempt.Quiz.Questions.FirstOrDefault(qq => qq.Id == answer.QuestionId);
                if (question == null) continue;

                var correctOption = question.AnswerOptions.FirstOrDefault(ao => ao.IsCorrect);
                var isCorrect = correctOption != null && answer.AnswerOptionId == correctOption.Id;

                if (isCorrect)
                    correctAnswers++;

                var userQuizAnswer = new UserQuizAnswer
                {
                    Id = Guid.NewGuid(),
                    UserQuizAttemptId = attemptId,
                    QuizQuestionId = answer.QuestionId,
                    SelectedAnswerOptionId = answer.AnswerOptionId,
                    IsCorrect = isCorrect
                };

                await _context.UserQuizAnswers.AddAsync(userQuizAnswer);

                questionResults.Add(new QuestionResultDto
                {
                    QuestionId = question.Id,
                    QuestionText = question.Text,
                    IsCorrect = isCorrect,
                    SelectedAnswerId = answer.AnswerOptionId,
                    CorrectAnswerId = correctOption?.Id
                });
            }

            // Calculate score
            var score = totalQuestions > 0 ? (double)correctAnswers / totalQuestions * 100 : 0;
            var isPassed = score >= attempt.Quiz.PassingScore;

            // Update attempt
            attempt.CompletedAt = DateTime.UtcNow;
            attempt.Score = Math.Round(score, 2);
            attempt.IsPassed = isPassed;
            attempt.TimeSpentMinutes = (int)(attempt.CompletedAt.Value - attempt.StartedAt).TotalMinutes;

            await _context.SaveChangesAsync();

            return new QuizResultDto
            {
                AttemptId = attemptId,
                Score = attempt.Score,
                IsPassed = isPassed,
                CorrectAnswers = correctAnswers,
                TotalQuestions = totalQuestions,
                TimeSpentMinutes = attempt.TimeSpentMinutes,
                QuestionResults = questionResults
            };
        }

        public async Task<List<QuizAttemptDto>> GetUserQuizAttemptsAsync(Guid userId, Guid? quizId = null)
        {
            var query = _context.UserQuizAttempts
                .Include(uqa => uqa.Quiz)
                .Where(uqa => uqa.UserId == userId && !uqa.IsDeleted);

            if (quizId.HasValue)
                query = query.Where(uqa => uqa.QuizId == quizId.Value);

            var attempts = await query
                .OrderByDescending(uqa => uqa.StartedAt)
                .ToListAsync();

            return attempts.Select(a => new QuizAttemptDto
            {
                Id = a.Id,
                QuizId = a.QuizId,
                QuizTitle = a.Quiz.Title,
                StartedAt = a.StartedAt,
                CompletedAt = a.CompletedAt,
                Score = a.Score,
                IsPassed = a.IsPassed,
                TimeSpentMinutes = a.TimeSpentMinutes
            }).ToList();
        }
    }
}

