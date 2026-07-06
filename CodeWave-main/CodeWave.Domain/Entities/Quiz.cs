using System;
using System.Collections.Generic;

namespace CodeWave.Domain.Entities
{
    public class Quiz
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int TimeLimitMinutes { get; set; } // Optional time limit
        public int PassingScore { get; set; } = 70; // Percentage required to pass

        public Course Course { get; set; }
        public ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
        public ICollection<UserQuizAttempt> UserQuizAttempts { get; set; } = new List<UserQuizAttempt>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}

