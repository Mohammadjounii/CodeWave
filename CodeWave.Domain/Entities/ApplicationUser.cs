using Microsoft.AspNetCore.Identity;

namespace CodeWave.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }

        public string Level { get; set; } // Beginner / Intermediate / Advanced
        public string LearningPath { get; set; } // Python / Java / Web

        // Onboarding data
        public string? Interests { get; set; }
        public string? Goal { get; set; }
        public string? SkillLevel { get; set; }
        public string? Motivation { get; set; }
        public int? WeeklyHours { get; set; }
        public string? PreferredLanguage { get; set; } // Python or Web Development
        public string? ProfilePictureUrl { get; set; } // Path to profile picture
        public bool IsAdmin { get; set; } = false; // Admin flag for authorization
        public bool OnboardingCompleted { get; set; } = false;

        public ICollection<UserAssesment> UserAssessments { get; set; } = new List<UserAssesment>();
        public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
        public ICollection<LessonCompletion> LessonCompletions { get; set; } = new List<LessonCompletion>();
        public ICollection<ExerciseSubmission> ExerciseSubmissions { get; set; } = new List<ExerciseSubmission>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<UserQuizAttempt> UserQuizAttempts { get; set; } = new List<UserQuizAttempt>();
        public CV? CV { get; set; }

    }
}
