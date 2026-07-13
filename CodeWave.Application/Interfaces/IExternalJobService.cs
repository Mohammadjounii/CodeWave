using CodeWave.Application.DTOs;

namespace CodeWave.Application.Interfaces
{
    public interface IExternalJobService
    {
        Task<List<ExternalJobDto>> SearchJobsAsync(string query = "software developer", List<string>? userSkills = null);
    }
}
