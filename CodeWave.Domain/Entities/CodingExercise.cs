using System;
using System.Collections.Generic;
using System.Text;

namespace CodeWave.Domain.Entities
{
    public class CodingExercise
    {
        public Guid Id { get; set; }
        public Guid LessonId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string StarterCode { get; set; }
        public string ExpectedOutput { get; set; }

        public Lesson Lesson { get; set; }
        public ICollection<ExerciseSubmission> ExerciseSubmissions { get; set; }
        public ICollection<ExerciseTestCase> TestCases { get; set; } = new List<ExerciseTestCase>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool isDeleted { get; set; } = false;
    }
}
