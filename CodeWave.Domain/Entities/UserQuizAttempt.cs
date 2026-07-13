using System;
using System.Collections.Generic;

namespace CodeWave.Domain.Entities
{
    public class UserQuizAttempt
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid QuizId { get; set; }
        public double Score { get; set; } // Percentage score
        public bool IsPassed { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int TimeSpentMinutes { get; set; }

        public ApplicationUser User { get; set; }
        public Quiz Quiz { get; set; }
        public ICollection<UserQuizAnswer> UserQuizAnswers { get; set; } = new List<UserQuizAnswer>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}

