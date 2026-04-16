using System;
using System.Collections.Generic;

namespace CodeWave.Application.Assessments.DTOs
{
    public class SubmitAssessmentDto
    {
        public Guid AssessmentId { get; set; }
        public Guid UserId { get; set; }

        // Key: QuestionId, Value: AnswerOptionId
        public Dictionary<Guid, Guid> Answers { get; set; }
    }

    public class SubmitAssessmentResultDto
    {
        public double Score { get; set; }
        public string Level { get; set; }
        public string LearningPath { get; set; }
    }

    public class SubmitExerciseRequest
    {
        public Guid ExerciseId { get; set; }
        public string SubmittedCode { get; set; }
        public string Output { get; set; }
    }

}
