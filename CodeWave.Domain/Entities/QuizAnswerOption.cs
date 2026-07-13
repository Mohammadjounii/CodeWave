using System;

namespace CodeWave.Domain.Entities
{
    public class QuizAnswerOption
    {
        public Guid Id { get; set; }
        public Guid QuizQuestionId { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

        public QuizQuestion QuizQuestion { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}

