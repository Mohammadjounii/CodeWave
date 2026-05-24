using System;
using System.Collections.Generic;
using System.Text;


namespace CodeWave.Domain.Entities
{
    public class Course
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        public string? DifficultyLevel { get; set; }


        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public string LearningPath { get; set; } = string.Empty;

        public Language? ProgrammingLanguage=Language.Java;


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }

    public enum Language
    {
        Python,         // 0
        Java,           // 1
        WebDevelopment  // 2
    }
}