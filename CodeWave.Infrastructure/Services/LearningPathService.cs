using CodeWave.Application.DTOs;
using CodeWave.Application.Interfaces;
using CodeWave.Application.ViewModels;
using CodeWave.Domain.Entities;

namespace CodeWave.Application.Services;

public class LearningPathService : ILearningPathService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ILessonRepository _lessonRepository;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IExerciseSubmissionRepository _exerciseSubmissionRepository;
    private readonly ILessonCompletionRepository _lessonCompletionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LearningPathService(
        ICourseRepository courseRepository,
        ILessonRepository lessonRepository,
        IExerciseRepository exerciseRepository,
        IExerciseSubmissionRepository exerciseSubmissionRepository,
        ILessonCompletionRepository lessonCompletionRepository,
        IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _lessonRepository = lessonRepository;
        _exerciseRepository = exerciseRepository;
        _exerciseSubmissionRepository = exerciseSubmissionRepository;
        _lessonCompletionRepository = lessonCompletionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<LearningPathViewModel?> GetCourseAsync(Guid courseId, Guid userId)
    {
        var course = await _courseRepository.GetByIdAsync(courseId);
        if (course == null)
        {
            return null;
        }

        var lessons = await _lessonRepository.GetByCourseAsync(courseId);
        var completed = await _lessonCompletionRepository.GetCompletedLessonIdsAsync(userId);
        var completedSet = completed.ToHashSet();

        // Resume at the first uncompleted lesson; fall back to the last if all done
        var nextLesson = lessons.FirstOrDefault(l => !completedSet.Contains(l.Id))
                         ?? lessons.LastOrDefault();

        return new LearningPathViewModel
        {
            Course = course,
            Lessons = lessons,
            CompletedLessonIds = completedSet,
            CurrentLessonId = nextLesson?.Id
        };
    }

    public async Task<LessonDto?> GetLessonAsync(Guid lessonId, Guid userId)
    {
        var lesson = await _lessonRepository.GetWithExercisesAsync(lessonId);
        if (lesson == null)
        {
            return null;
        }

        var exerciseIds = lesson.CodingExercises.Select(e => e.Id).ToList();
        var submissions = await _exerciseSubmissionRepository.GetLatestForUserAsync(userId, exerciseIds);
        var solvedLookup = submissions
            .GroupBy(s => s.ExerciseId)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.SubmissionDate).First().IsCorrect);

        return new LessonDto
        {
            Id = lesson.Id,
            Title = lesson.Title,
            Content = lesson.Content,
            VideoUrl = lesson.VideoUrl,
            ImageUrl = lesson.ImageUrl,
            Exercises = lesson.CodingExercises.Select(e => new ExerciseDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                StarterCode = e.StarterCode,
                ExpectedOutput = e.ExpectedOutput,
                IsSolved = solvedLookup.TryGetValue(e.Id, out var solved) && solved
            })
        };
    }

    public async Task<ServiceResult> CompleteLessonAsync(Guid lessonId, Guid userId, bool requireExercisesSolved = true)
    {
        var lesson = await _lessonRepository.GetWithExercisesAsync(lessonId);
        if (lesson == null)
        {
            return new ServiceResult { Success = false, Message = "Lesson not found." };
        }

        // Enforce sequential access: previous lesson must be completed first
        var allLessons = await _lessonRepository.GetByCourseAsync(lesson.CourseId);
        var orderedLessons = allLessons.OrderBy(l => l.OrderNumber).ToList();
        var currentIndex = orderedLessons.FindIndex(l => l.Id == lessonId);
        if (currentIndex > 0)
        {
            var previousLessonId = orderedLessons[currentIndex - 1].Id;
            var prevCompletion = await _lessonCompletionRepository.GetAsync(userId, previousLessonId);
            if (prevCompletion?.IsCompleted != true)
            {
                return new ServiceResult { Success = false, Message = "Previous lesson must be completed first." };
            }
        }

        if (requireExercisesSolved && lesson.CodingExercises.Any())
        {
            var exerciseIds = lesson.CodingExercises.Select(e => e.Id).ToList();
            var submissions = await _exerciseSubmissionRepository.GetLatestForUserAsync(userId, exerciseIds);
            var solvedCount = submissions.Count(s => s.IsCorrect);

            if (solvedCount != exerciseIds.Count)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = "All exercises must be solved before completing this lesson."
                };
            }
        }

        var existing = await _lessonCompletionRepository.GetAsync(userId, lessonId);
        if (existing == null)
        {
            var completion = new LessonCompletion
            {
                Id = Guid.NewGuid(),
                LessonId = lessonId,
                UserId = userId,
                IsCompleted = true,
                CompletionDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
            await _lessonCompletionRepository.AddAsync(completion);
        }
        else
        {
            existing.IsCompleted = true;
            existing.CompletionDate = DateTime.UtcNow;
        }

        await _unitOfWork.SaveChangesAsync();
        return new ServiceResult { Success = true };
    }

    public async Task<SubmitExerciseResultDto> SubmitExerciseAsync(SubmitExerciseRequestDto model, Guid userId)
    {
        var exercise = await _exerciseRepository.GetByIdAsync(model.ExerciseId);
        if (exercise == null)
        {
            return new SubmitExerciseResultDto { Success = false, Message = "Exercise not found." };
        }

        var expected = (exercise.ExpectedOutput ?? string.Empty).Trim();
        var actual = (model.Output ?? string.Empty).Trim();
        var isCorrect = expected == actual;

        var existing = await _exerciseSubmissionRepository.GetAsync(userId, model.ExerciseId);
        if (existing == null)
        {
            var submission = new ExerciseSubmission
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ExerciseId = model.ExerciseId,
                SubmittedCode = model.SubmittedCode ?? string.Empty,
                Output = model.Output ?? string.Empty,
                IsCorrect = isCorrect,
                SubmissionDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
            await _exerciseSubmissionRepository.AddAsync(submission);
        }
        else
        {
            existing.SubmittedCode = model.SubmittedCode ?? string.Empty;
            existing.Output = model.Output ?? string.Empty;
            existing.IsCorrect = existing.IsCorrect || isCorrect; // never regress from correct → wrong
            existing.SubmissionDate = DateTime.UtcNow;
        }

        await _unitOfWork.SaveChangesAsync();

        return new SubmitExerciseResultDto
        {
            Success = true,
            IsCorrect = isCorrect,
            Output = model.Output,
            Message = isCorrect ? "Correct solution!" : "Incorrect output. Try again."
        };
    }

    public async Task<FocusModeViewModel?> GetFocusModeAsync(Guid lessonId, Guid userId)
    {
        var lesson = await _lessonRepository.GetWithExercisesAsync(lessonId);
        if (lesson == null)
        {
            return null;
        }

        // Get the course for this lesson
        var course = await _courseRepository.GetByIdAsync(lesson.CourseId);
        if (course == null)
        {
            return null;
        }

        // Get all lessons in the course ordered by OrderNumber
        var allLessons = await _lessonRepository.GetByCourseAsync(course.Id);
        var orderedLessons = allLessons.OrderBy(l => l.OrderNumber).ToList();
        
        // Find current lesson index
        var currentIndex = orderedLessons.FindIndex(l => l.Id == lessonId);
        
        // Get previous and next lesson IDs
        Guid? previousLessonId = null;
        Guid? nextLessonId = null;
        
        if (currentIndex > 0)
        {
            previousLessonId = orderedLessons[currentIndex - 1].Id;
        }
        
        if (currentIndex < orderedLessons.Count - 1)
        {
            nextLessonId = orderedLessons[currentIndex + 1].Id;
        }

        var exerciseIds = lesson.CodingExercises.Select(e => e.Id).ToList();
        var submissions = await _exerciseSubmissionRepository.GetLatestForUserAsync(userId, exerciseIds);
        var solvedLookup = submissions
            .GroupBy(s => s.ExerciseId)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.SubmissionDate).First().IsCorrect);

        return new FocusModeViewModel
        {
            Lesson = lesson,
            Course = course,
            Exercises = lesson.CodingExercises.ToList(),
            SolvedLookup = solvedLookup,
            PreviousLessonId = previousLessonId,
            NextLessonId = nextLessonId
        };
    }
}

