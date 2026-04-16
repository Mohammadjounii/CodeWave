using System;
using System.Collections.Generic;

namespace CodeWave.Domain.Entities
{
    public class QuizQuestion
    {
        public Guid Id { get; set; }
        public Guid QuizId { get; set; }
        public string Text { get; set; }
        public string Difficulty { get; set; } // Easy, Medium, Hard
        public int OrderNumber { get; set; }

        public Quiz Quiz { get; set; }
        public ICollection<QuizAnswerOption> AnswerOptions { get; set; } = new List<QuizAnswerOption>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}

