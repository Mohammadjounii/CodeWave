using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Services;

public class JobApplicationService : IJobApplicationService
{
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public JobApplicationService(ApplicationDbContext context, IUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> HasUserAppliedAsync(Guid userId, Guid jobOfferId)
    {
        return await _context.JobApplications
            .AnyAsync(ja => ja.UserId == userId && ja.JobOfferId == jobOfferId);
    }

    public async Task<List<Guid>> GetUserApplicationIdsAsync(Guid userId)
    {
        return await _context.JobApplications
            .Where(ja => ja.UserId == userId)
            .Select(ja => ja.JobOfferId)
            .ToListAsync();
    }

    public async Task<List<JobApplication>> GetUserApplicationsAsync(Guid userId)
    {
        return await _context.JobApplications
            .Where(ja => ja.UserId == userId)
            .Include(ja => ja.JobOffer)
            .OrderByDescending(ja => ja.AppliedDate)
            .ToListAsync();
    }

    public async Task<JobApplication?> GetExistingApplicationAsync(Guid userId, Guid jobOfferId)
    {
        return await _context.JobApplications
            .FirstOrDefaultAsync(ja => ja.UserId == userId && ja.JobOfferId == jobOfferId);
    }

    public async Task<JobApplication> CreateApplicationAsync(JobApplication application)
    {
        _context.JobApplications.Add(application);
        await _unitOfWork.SaveChangesAsync();
        return application;
    }

    public async Task<bool> WithdrawApplicationAsync(Guid applicationId, Guid userId)
    {
        var application = await _context.JobApplications
            .FirstOrDefaultAsync(a => a.Id == applicationId && a.UserId == userId);

        if (application == null || application.Status != "Pending")
            return false;

        _context.JobApplications.Remove(application);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

