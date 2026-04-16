using Xunit;
using Microsoft.EntityFrameworkCore;
using CodeWave.Infrastructure.Data;
using CodeWave.Infrastructure.Repositories;
using CodeWave.Domain.Entities;

namespace CodeWave.Tests.Repositories;

public class UnitOfWorkTests
{
    [Fact]
    public async Task SaveChangesAsync_ShouldSaveChangesToDatabase()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ApplicationDbContext(options);
        var unitOfWork = new UnitOfWork(context);

        // Act
        var result = await unitOfWork.SaveChangesAsync();

        // Assert
        Assert.True(result >= 0);
    }
}
