using System;

namespace CodeWave.Domain.Entities
{
    public class UserQuizAnswer
    {
        public Guid Id { get; set; }
        public Guid UserQuizAttemptId { get; set; }
        public Guid QuizQuestionId { get; set; }
        public Guid? SelectedAnswerOptionId { get; set; }
        public bool IsCorrect { get; set; }

        public UserQuizAttempt UserQuizAttempt { get; set; }
        public QuizQuestion QuizQuestion { get; set; }
        public QuizAnswerOption? SelectedAnswerOption { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}

