using CodeWave.Domain.Entities;

namespace CodeWave.Application.Interfaces;

public interface ICVService
{
    Task<CV?> GetCVByUserIdAsync(Guid userId);
    Task<CV> CreateOrUpdateCVAsync(CV cv);
}

