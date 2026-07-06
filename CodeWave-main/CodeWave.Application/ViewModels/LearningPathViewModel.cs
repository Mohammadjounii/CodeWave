using System;
using System.Collections.Generic;
using CodeWave.Domain.Entities;

namespace CodeWave.Application.ViewModels
{
    public class LearningPathViewModel
    {
        public Course Course { get; set; }

        public List<Lesson> Lessons { get; set; } = new();

        // Which lessons this user has completed
        public HashSet<Guid> CompletedLessonIds { get; set; } = new();

        // The lesson to open initially
        public Guid? CurrentLessonId { get; set; }

        public int TotalLessons => Lessons?.Count ?? 0;
        public int CompletedLessonsCount => CompletedLessonIds?.Count ?? 0;

        public int CompletionPercent =>
            TotalLessons == 0 ? 0 : (int)Math.Round(100.0 * CompletedLessonsCount / TotalLessons);
    }
}
