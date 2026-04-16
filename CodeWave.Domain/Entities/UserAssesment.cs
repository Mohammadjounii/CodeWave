
namespace CodeWave.Domain.Entities
{
    public class UserAssesment
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public Guid AssessmentId { get; set; }

        public double Score { get; set; }
        public string ResultLevel { get; set; }
        public string AssignedLearningPath { get; set; }

        public DateTime DateTaken { get; set; }

        public ApplicationUser User { get; set; }
        public Assessment Assessment { get; set; }

        public ICollection<UserAnswer> UserAnswers { get; set; } = new List<UserAnswer>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool isDeleted { get; set; } = false;
    }
}
