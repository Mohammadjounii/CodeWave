using CodeWave.Domain.Entities;

namespace CodeWave.Application.Interfaces
{
    public interface IUserReportService
    {
        Task SubmitReportAsync(UserReport report);
        Task<List<UserReport>> GetAllReportsAsync();
        Task<bool> UpdateStatusAsync(Guid reportId, string status);
        Task<bool> DeleteReportAsync(Guid reportId);
    }
}
