using System;
using System.Collections.Generic;

namespace CodeWave.Application.Assessments.DTOs
{
    public class TakeAssessmentDto
    {
        public Guid AssessmentId { get; set; }
        public string Title { get; set; }

        public List<QuestionDto> Questions { get; set; }
    }

    public class QuestionDto
    {
        public Guid QuestionId { get; set; }
        public string Text { get; set; }

        public List<AnswerOptionDto> Options { get; set; }
    }

    public class AnswerOptionDto
    {
        public Guid AnswerOptionId { get; set; }
        public string Text { get; set; }
    }
}
