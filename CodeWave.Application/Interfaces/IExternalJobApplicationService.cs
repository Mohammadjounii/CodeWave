using CodeWave.Domain.Entities;

namespace CodeWave.Application.Interfaces
{
    public interface IExternalJobApplicationService
    {
        Task<bool> HasUserAppliedAsync(Guid userId, string externalJobId);
        Task SaveApplicationAsync(ExternalJobApplication application);
        Task<List<ExternalJobApplication>> GetUserApplicationsAsync(Guid userId);
        Task<List<ExternalJobApplication>> GetAllApplicationsAsync();
    }
}
