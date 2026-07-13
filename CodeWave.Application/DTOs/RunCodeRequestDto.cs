namespace CodeWave.Application.DTOs;

public class RunCodeRequestDto
{
    public string? Language { get; set; }
    public string? Code { get; set; }
    public Guid? ExerciseId { get; set; } // Made nullable to allow running code without an exercise
}

