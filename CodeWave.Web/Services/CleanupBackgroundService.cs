using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace CodeWave.Web.Services;

public class CleanupBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _period = TimeSpan.FromHours(24);

    public CleanupBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                await CleanupOldTempFiles(context, stoppingToken);
                await CleanupOldDeletedRecords(context, stoppingToken);

                Log.Information("Background cleanup service completed successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in background cleanup service");
            }

            await Task.Delay(_period, stoppingToken);
        }
    }

    private async Task CleanupOldTempFiles(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-7);

        var oldSubmissions = await context.ExerciseSubmissions
            .Where(s => s.isDeleted && s.CreatedAt < cutoffDate)
            .ToListAsync(cancellationToken);

        if (oldSubmissions.Any())
        {
            context.ExerciseSubmissions.RemoveRange(oldSubmissions);
            await context.SaveChangesAsync(cancellationToken);
            Log.Information("Permanently removed {Count} old soft-deleted exercise submissions", oldSubmissions.Count);
        }
    }

    private async Task CleanupOldDeletedRecords(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-90);

        // Load all sets sequentially (EF Core DbContext is not thread-safe)
        var courses    = await context.Courses.Where(c => c.IsDeleted && c.CreatedAt < cutoffDate).ToListAsync(cancellationToken);
        var lessons    = await context.Lessons.Where(l => l.isDeleted && l.CreatedAt < cutoffDate).ToListAsync(cancellationToken);
        var exercises  = await context.CodingExercises.Where(e => e.isDeleted && e.CreatedAt < cutoffDate).ToListAsync(cancellationToken);
        var jobs       = await context.JobOffers.Where(j => j.isDeleted && j.CreatedAt < cutoffDate).ToListAsync(cancellationToken);
        var projects   = await context.Projects.Where(p => p.isDeleted && p.CreatedAt < cutoffDate).ToListAsync(cancellationToken);

        var totalRemoved = courses.Count + lessons.Count + exercises.Count + jobs.Count + projects.Count;
        if (totalRemoved == 0) return;

        if (courses.Any())   context.Courses.RemoveRange(courses);
        if (lessons.Any())   context.Lessons.RemoveRange(lessons);
        if (exercises.Any()) context.CodingExercises.RemoveRange(exercises);
        if (jobs.Any())      context.JobOffers.RemoveRange(jobs);
        if (projects.Any())  context.Projects.RemoveRange(projects);

        await context.SaveChangesAsync(cancellationToken);
        Log.Information("Permanently removed {Count} soft-deleted records older than 90 days", totalRemoved);
    }
}
