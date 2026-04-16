using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace CodeWave.Domain.Entities
{
    public class UserAnswer
    {
        public Guid Id { get; set; }

        // The UserAssessment this answer belongs to
        public Guid UserAssessmentId { get; set; }

        // The Question the user answered
        public Guid QuestionId { get; set; }

        // The specific Option the user selected
        public Guid AnswerOptionId { get; set; }

        // Whether the selected answer is correct
        public bool IsCorrect { get; set; }

        // Navigation
        public UserAssesment UserAssessment { get; set; }
        public Question Question { get; set; }
        public AnswerOption AnswerOption { get; set; }
    }
}

