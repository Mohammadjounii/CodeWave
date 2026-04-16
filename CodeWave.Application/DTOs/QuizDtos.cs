using System;
using System.Collections.Generic;

namespace CodeWave.Application.DTOs
{
    public class QuizDto
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TimeLimitMinutes { get; set; }
        public int PassingScore { get; set; }
        public int QuestionCount { get; set; }
        public List<QuizQuestionDto> Questions { get; set; } = new List<QuizQuestionDto>();
    }

    public class QuizQuestionDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string Difficulty { get; set; }
        public int OrderNumber { get; set; }
        public List<QuizAnswerOptionDto> AnswerOptions { get; set; } = new List<QuizAnswerOptionDto>();
    }

    public class QuizAnswerOptionDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class QuizAttemptDto
    {
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public string QuizTitle { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public double? Score { get; set; }
        public bool? IsPassed { get; set; }
        public int TimeSpentMinutes { get; set; }
    }

    public class QuizAnswerDto
    {
        public Guid QuestionId { get; set; }
        public Guid? AnswerOptionId { get; set; }
    }

    public class QuizResultDto
    {
        public Guid AttemptId { get; set; }
        public double Score { get; set; }
        public bool IsPassed { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public int TimeSpentMinutes { get; set; }
        public List<QuestionResultDto> QuestionResults { get; set; } = new List<QuestionResultDto>();
    }

    public class QuestionResultDto
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
        public bool IsCorrect { get; set; }
        public Guid? SelectedAnswerId { get; set; }
        public Guid? CorrectAnswerId { get; set; }
    }
}

