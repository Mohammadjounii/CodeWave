namespace CodeWave.Application.DTOs;

public class LessonDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? VideoUrl { get; set; }
    public string? ImageUrl { get; set; }
    public IEnumerable<ExerciseDto> Exercises { get; set; } = Enumerable.Empty<ExerciseDto>();
}

public class ExerciseDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? StarterCode { get; set; }
    public string? ExpectedOutput { get; set; }
    public bool IsSolved { get; set; }
}

public class SubmitExerciseRequestDto
{
    public Guid ExerciseId { get; set; }
    public string? SubmittedCode { get; set; }
    public string? Output { get; set; }
}

public class ServiceResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}

public class TestCaseResultDto
{
    public Guid TestCaseId { get; set; }
    public int OrderNumber { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool Passed { get; set; }
    public string? ExpectedOutput { get; set; }
    public string? ActualOutput { get; set; }
    public string? Input { get; set; }
}

public class SubmitExerciseResultDto : ServiceResult
{
    public bool IsCorrect { get; set; }
    public string? Output { get; set; }
    public List<TestCaseResultDto> TestCaseResults { get; set; } = new List<TestCaseResultDto>();
    public int PassedTests { get; set; }
    public int TotalTests { get; set; }
}

