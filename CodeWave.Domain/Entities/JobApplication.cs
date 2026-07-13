using System;

namespace CodeWave.Domain.Entities
{
    public class JobApplication
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid JobOfferId { get; set; }
        public DateTime AppliedDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending"; // Pending, Reviewed, Accepted, Rejected
        public string CoverLetter { get; set; }
        public double MatchPercentage { get; set; } // Skill match percentage
        
        // Navigation properties
        public ApplicationUser User { get; set; }
        public JobOffer JobOffer { get; set; }
    }
}

