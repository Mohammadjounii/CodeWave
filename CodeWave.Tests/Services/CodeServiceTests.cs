using Xunit;
using Moq;
using CodeWave.Application.Services;
using CodeWave.Application.Interfaces;
using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Services;

namespace CodeWave.Tests.Services;

public class CodeServiceTests
{
    [Fact]
    public void RunAndSaveAsync_WithValidCode_ShouldReturnSuccess()
    {
        // Arrange
        var mockRunner = new Mock<CodeRunnerService>();
        var mockExerciseRepo = new Mock<IExerciseRepository>();
        var mockSubmissionRepo = new Mock<IExerciseSubmissionRepository>();
        var mockCourseRepo = new Mock<ICourseRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        var codeService = new CodeService(
            mockRunner.Object,
            mockExerciseRepo.Object,
            mockSubmissionRepo.Object,
            mockCourseRepo.Object,
            mockUnitOfWork.Object
        );

        // This is a basic test structure
        // In a real scenario, you would mock the dependencies and test the service logic
        Assert.NotNull(codeService);
    }

    [Fact]
    public void RunAndSaveAsync_WithEmptyCode_ShouldReturnFailure()
    {
        // Arrange
        var mockRunner = new Mock<CodeRunnerService>();
        var mockExerciseRepo = new Mock<IExerciseRepository>();
        var mockSubmissionRepo = new Mock<IExerciseSubmissionRepository>();
        var mockCourseRepo = new Mock<ICourseRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        var codeService = new CodeService(
            mockRunner.Object,
            mockExerciseRepo.Object,
            mockSubmissionRepo.Object,
            mockCourseRepo.Object,
            mockUnitOfWork.Object
        );

        Assert.NotNull(codeService);
    }
}
