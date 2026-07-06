using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Services
{
    public class ExternalJobApplicationService : IExternalJobApplicationService
    {
        private readonly ApplicationDbContext _context;

        public ExternalJobApplicationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasUserAppliedAsync(Guid userId, string externalJobId)
        {
            return await _context.ExternalJobApplications
                .AnyAsync(e => e.UserId == userId && e.ExternalJobId == externalJobId);
        }

        public async Task SaveApplicationAsync(ExternalJobApplication application)
        {
            _context.ExternalJobApplications.Add(application);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ExternalJobApplication>> GetUserApplicationsAsync(Guid userId)
        {
            return await _context.ExternalJobApplications
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.AppliedDate)
                .ToListAsync();
        }

        public async Task<List<ExternalJobApplication>> GetAllApplicationsAsync()
        {
            return await _context.ExternalJobApplications
                .Include(e => e.User)
                .OrderByDescending(e => e.AppliedDate)
                .ToListAsync();
        }
    }
}
