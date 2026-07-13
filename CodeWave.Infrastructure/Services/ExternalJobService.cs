using System.Net.Http;
using System.Text.Json;
using CodeWave.Application.DTOs;
using CodeWave.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace CodeWave.Infrastructure.Services
{
    public class ExternalJobService : IExternalJobService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;

        public ExternalJobService(HttpClient httpClient, IConfiguration configuration, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _cache = cache;
        }

        public async Task<List<ExternalJobDto>> SearchJobsAsync(string query = "software developer", List<string>? userSkills = null)
        {
            var cacheKey = $"jsearch_{query.ToLower().Replace(" ", "_")}";

            if (_cache.TryGetValue(cacheKey, out List<ExternalJobDto>? cached) && cached != null)
                return ApplySkillMatch(cached, userSkills);

            try
            {
                var apiKey = _configuration["JSearch:ApiKey"];
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-rapidapi-key", apiKey);
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("x-rapidapi-host", "jsearch.p.rapidapi.com");

                var url = $"https://jsearch.p.rapidapi.com/search?query={Uri.EscapeDataString(query)}&num_pages=1&page=1&date_posted=month";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return new List<ExternalJobDto>();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<JSearchResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Data == null)
                    return new List<ExternalJobDto>();

                var jobs = result.Data.Select(j => new ExternalJobDto
                {
                    JobId = j.JobId,
                    EmployerName = j.EmployerName,
                    EmployerLogo = j.EmployerLogo,
                    JobPublisher = j.JobPublisher,
                    JobTitle = j.JobTitle,
                    JobApplyLink = j.JobApplyLink,
                    JobDescription = j.JobDescription?.Length > 3000
                        ? j.JobDescription[..3000] + "..."
                        : j.JobDescription ?? string.Empty,
                    JobIsRemote = j.JobIsRemote,
                    JobCity = j.JobCity,
                    JobState = j.JobState,
                    JobCountry = j.JobCountry ?? string.Empty,
                    JobEmploymentType = FormatEmploymentType(j.JobEmploymentType),
                    JobRequiredSkills = j.JobRequiredSkills ?? new List<string>(),
                    JobMinSalary = j.JobMinSalary,
                    JobMaxSalary = j.JobMaxSalary,
                    JobSalaryCurrency = j.JobSalaryCurrency,
                    JobSalaryPeriod = j.JobSalaryPeriod,
                    JobPostedAt = string.IsNullOrEmpty(j.JobPostedAtDatetimeUtc)
                        ? null
                        : DateTime.TryParse(j.JobPostedAtDatetimeUtc, out var dt) ? dt : null
                }).ToList();

                _cache.Set(cacheKey, jobs, TimeSpan.FromMinutes(30));
                return ApplySkillMatch(jobs, userSkills);
            }
            catch
            {
                return new List<ExternalJobDto>();
            }
        }

        private static List<ExternalJobDto> ApplySkillMatch(List<ExternalJobDto> jobs, List<string>? userSkills)
        {
            if (userSkills == null || userSkills.Count == 0)
                return jobs;

            var result = jobs.Select(job =>
            {
                var clone = new ExternalJobDto
                {
                    JobId = job.JobId,
                    EmployerName = job.EmployerName,
                    EmployerLogo = job.EmployerLogo,
                    JobPublisher = job.JobPublisher,
                    JobTitle = job.JobTitle,
                    JobApplyLink = job.JobApplyLink,
                    JobDescription = job.JobDescription,
                    JobIsRemote = job.JobIsRemote,
                    JobCity = job.JobCity,
                    JobState = job.JobState,
                    JobCountry = job.JobCountry,
                    JobEmploymentType = job.JobEmploymentType,
                    JobRequiredSkills = job.JobRequiredSkills,
                    JobMinSalary = job.JobMinSalary,
                    JobMaxSalary = job.JobMaxSalary,
                    JobSalaryCurrency = job.JobSalaryCurrency,
                    JobSalaryPeriod = job.JobSalaryPeriod,
                    JobPostedAt = job.JobPostedAt
                };

                if (!clone.JobRequiredSkills.Any())
                {
                    clone.MatchPercentage = 50;
                }
                else
                {
                    var matched = clone.JobRequiredSkills.Count(rs =>
                        userSkills.Any(us =>
                            us.Equals(rs, StringComparison.OrdinalIgnoreCase) ||
                            us.Contains(rs, StringComparison.OrdinalIgnoreCase) ||
                            rs.Contains(us, StringComparison.OrdinalIgnoreCase)));

                    clone.MatchPercentage = Math.Round((double)matched / clone.JobRequiredSkills.Count * 100, 1);
                }

                return clone;
            }).OrderByDescending(j => j.MatchPercentage).ToList();

            return result;
        }

        private static string FormatEmploymentType(string? type) => type switch
        {
            "FULLTIME" => "Full-time",
            "PARTTIME" => "Part-time",
            "CONTRACTOR" => "Contract",
            "INTERN" => "Internship",
            _ => type ?? "Full-time"
        };
    }
}
