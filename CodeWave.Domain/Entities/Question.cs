using System;
using System.Collections.Generic;
using System.Text;

namespace CodeWave.Domain.Entities
{
    public class Question
    {
        public Guid Id { get; set; }
        public Guid AssessmentId { get; set; }

        public string Text { get; set; }
        public string Difficulty { get; set; }

        public ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>(); // REQUIRED

        public Assessment Assessment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool isDeleted { get; set; } = false;
    }

}
