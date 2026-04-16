using CodeWave.Application.DTOs;

namespace CodeWave.Application.Interfaces;

public interface ICodeService
{
    Task<SubmitExerciseResultDto> RunAndSaveAsync(RunCodeRequestDto request, Guid userId);
}

