using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Services
{
    public class UserReportService : IUserReportService
    {
        private readonly ApplicationDbContext _db;

        public UserReportService(ApplicationDbContext db) => _db = db;

        public async Task SubmitReportAsync(UserReport report)
        {
            _db.UserReports.Add(report);
            await _db.SaveChangesAsync();
        }

        public Task<List<UserReport>> GetAllReportsAsync() =>
            _db.UserReports
                .Where(r => !r.IsDeleted)
                .OrderByDescending(r => r.SubmittedAt)
                .ToListAsync();

        public async Task<bool> UpdateStatusAsync(Guid reportId, string status)
        {
            var report = await _db.UserReports.FindAsync(reportId);
            if (report == null) return false;
            report.Status = status;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteReportAsync(Guid reportId)
        {
            var report = await _db.UserReports.FindAsync(reportId);
            if (report == null) return false;
            report.IsDeleted = true;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
