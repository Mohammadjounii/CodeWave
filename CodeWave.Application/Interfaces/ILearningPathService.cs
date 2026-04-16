using CodeWave.Application.DTOs;
using CodeWave.Application.ViewModels;

namespace CodeWave.Application.Interfaces;

public interface ILearningPathService
{
    Task<LearningPathViewModel?> GetCourseAsync(Guid courseId, Guid userId);
    Task<LessonDto?> GetLessonAsync(Guid lessonId, Guid userId);
    Task<ServiceResult> CompleteLessonAsync(Guid lessonId, Guid userId, bool requireExercisesSolved = true);
    Task<SubmitExerciseResultDto> SubmitExerciseAsync(SubmitExerciseRequestDto model, Guid userId);
    Task<FocusModeViewModel?> GetFocusModeAsync(Guid lessonId, Guid userId);
}

