using CodeWave.Domain.Entities;

namespace CodeWave.Application.Interfaces;

public interface IJobOfferService
{
    Task<List<JobOffer>> GetActiveJobOffersAsync(string? searchTerm = null);
    Task<JobOffer?> GetJobOfferByIdAsync(Guid id);
}

