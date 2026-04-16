using CodeWave.Domain.Entities;

namespace CodeWave.Application.Interfaces;

public interface IJobApplicationService
{
    Task<bool> HasUserAppliedAsync(Guid userId, Guid jobOfferId);
    Task<List<Guid>> GetUserApplicationIdsAsync(Guid userId);
    Task<List<JobApplication>> GetUserApplicationsAsync(Guid userId);
    Task<JobApplication?> GetExistingApplicationAsync(Guid userId, Guid jobOfferId);
    Task<JobApplication> CreateApplicationAsync(JobApplication application);
    Task<bool> WithdrawApplicationAsync(Guid applicationId, Guid userId);
}

