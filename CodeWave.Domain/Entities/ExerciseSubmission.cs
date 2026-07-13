using System;
using System.Collections.Generic;
using System.Text;

namespace CodeWave.Domain.Entities
{
    public class ExerciseSubmission
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public Guid ExerciseId { get; set; }

        public string SubmittedCode { get; set; }
        public string Output { get; set; }
        public bool IsCorrect { get; set; }

        public DateTime SubmissionDate { get; set; }

        public ApplicationUser User { get; set; }
        public CodingExercise Exercise { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool isDeleted { get; set; } = false;
    }
}
