using System;

namespace CodeWave.Domain.Entities
{
    public class ExerciseTestCase
    {
        public Guid Id { get; set; }
        public Guid ExerciseId { get; set; }
        
        public string Input { get; set; } // Input data for the test case (can be empty for simple output checks)
        public string ExpectedOutput { get; set; } // Expected output for this test case
        public string Description { get; set; } // Description of what this test case validates
        public int OrderNumber { get; set; } // Order of test cases
        
        public CodingExercise Exercise { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}

