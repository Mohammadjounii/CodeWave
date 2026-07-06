using System.Text.Json.Serialization;

namespace CodeWave.Application.DTOs
{
    public class ExternalJobDto
    {
        public string JobId { get; set; } = string.Empty;
        public string EmployerName { get; set; } = string.Empty;
        public string? EmployerLogo { get; set; }
        public string? JobPublisher { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string JobApplyLink { get; set; } = string.Empty;
        public string JobDescription { get; set; } = string.Empty;
        public bool JobIsRemote { get; set; }
        public string? JobCity { get; set; }
        public string? JobState { get; set; }
        public string JobCountry { get; set; } = string.Empty;
        public string JobEmploymentType { get; set; } = string.Empty;
        public List<string> JobRequiredSkills { get; set; } = new();
        public double? JobMinSalary { get; set; }
        public double? JobMaxSalary { get; set; }
        public string? JobSalaryCurrency { get; set; }
        public string? JobSalaryPeriod { get; set; }
        public DateTime? JobPostedAt { get; set; }
        public double MatchPercentage { get; set; }
    }

    public class JSearchResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public List<JSearchJob> Data { get; set; } = new();
    }

    public class JSearchJob
    {
        [JsonPropertyName("job_id")]
        public string JobId { get; set; } = string.Empty;

        [JsonPropertyName("employer_name")]
        public string EmployerName { get; set; } = string.Empty;

        [JsonPropertyName("employer_logo")]
        public string? EmployerLogo { get; set; }

        [JsonPropertyName("job_publisher")]
        public string? JobPublisher { get; set; }

        [JsonPropertyName("job_employment_type")]
        public string? JobEmploymentType { get; set; }

        [JsonPropertyName("job_title")]
        public string JobTitle { get; set; } = string.Empty;

        [JsonPropertyName("job_apply_link")]
        public string JobApplyLink { get; set; } = string.Empty;

        [JsonPropertyName("job_description")]
        public string JobDescription { get; set; } = string.Empty;

        [JsonPropertyName("job_is_remote")]
        public bool JobIsRemote { get; set; }

        [JsonPropertyName("job_posted_at_datetime_utc")]
        public string? JobPostedAtDatetimeUtc { get; set; }

        [JsonPropertyName("job_city")]
        public string? JobCity { get; set; }

        [JsonPropertyName("job_state")]
        public string? JobState { get; set; }

        [JsonPropertyName("job_country")]
        public string? JobCountry { get; set; }

        [JsonPropertyName("job_required_skills")]
        public List<string>? JobRequiredSkills { get; set; }

        [JsonPropertyName("job_min_salary")]
        public double? JobMinSalary { get; set; }

        [JsonPropertyName("job_max_salary")]
        public double? JobMaxSalary { get; set; }

        [JsonPropertyName("job_salary_currency")]
        public string? JobSalaryCurrency { get; set; }

        [JsonPropertyName("job_salary_period")]
        public string? JobSalaryPeriod { get; set; }
    }
}
