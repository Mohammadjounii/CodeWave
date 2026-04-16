using System;
using System.Collections.Generic;
using System.Text;

namespace CodeWave.Domain.Entities
{
    public class Lesson
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }

        public string Title { get; set; } = null!;    // Required
        public string Content { get; set; } = null!;  // Required
        public string? VideoUrl { get; set; }
        public string? ImageUrl { get; set; }

        public int OrderNumber { get; set; }

        // Navigation properties
        public ICollection<CodingExercise> CodingExercises { get; set; } = new List<CodingExercise>();
        public ICollection<LessonCompletion> LessonCompletions { get; set; } = new List<LessonCompletion>();

        public Course? Course { get; set; }  // Nullable to avoid EF insert issues

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool isDeleted { get; set; } = false;
    }
}
