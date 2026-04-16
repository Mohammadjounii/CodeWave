using System;
using System.Collections.Generic;
using System.Text;

namespace CodeWave.Domain.Entities
{
    public class AnswerOption
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }

        public string Text { get; set; } = string.Empty; // FIXED: Initialized to non-null value
        public bool IsCorrect { get; set; }

        public Question? Question { get; set; } // FIXED: Declared as nullable

        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>(); // REQUIRED

        public DateTime CreatedAt = new DateTime(2025, 12, 4);
        public bool isDeleted { get; set; } = false;
    }
}
