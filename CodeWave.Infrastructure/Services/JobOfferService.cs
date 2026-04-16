using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Services;

public class JobOfferService : IJobOfferService
{
    private readonly ApplicationDbContext _context;

    public JobOfferService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<JobOffer>> GetActiveJobOffersAsync(string? searchTerm = null)
    {
        var query = _context.JobOffers
            .Where(j => !j.isDeleted && j.Deadline > DateTime.UtcNow)
            .OrderByDescending(j => j.PostedDate)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            // Use EF.Functions.Like for case-insensitive search (MySQL compatible)
            // MySQL LIKE is case-insensitive by default for most collations
            var searchPattern = $"%{searchTerm}%";
            
            query = query.Where(j => 
                (j.JobTitle != null && EF.Functions.Like(j.JobTitle, searchPattern)) ||
                (j.Company != null && EF.Functions.Like(j.Company, searchPattern)) ||
                (j.Description != null && EF.Functions.Like(j.Description, searchPattern)) ||
                (j.RequiredSkills != null && EF.Functions.Like(j.RequiredSkills, searchPattern)));
        }

        return await query.ToListAsync();
    }

    public async Task<JobOffer?> GetJobOfferByIdAsync(Guid id)
    {
        return await _context.JobOffers
            .FirstOrDefaultAsync(j => j.Id == id && !j.isDeleted);
    }
}

