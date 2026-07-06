using System;

namespace CodeWave.Domain.Entities
{
    public class ExternalJobApplication
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ExternalJobId { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string EmployerName { get; set; } = string.Empty;
        public string? EmployerLogo { get; set; }
        public string? JobLocation { get; set; }
        public string ApplyLink { get; set; } = string.Empty;
        public double MatchPercentage { get; set; }
        public DateTime AppliedDate { get; set; } = DateTime.UtcNow;

        public ApplicationUser User { get; set; } = null!;
    }
}
