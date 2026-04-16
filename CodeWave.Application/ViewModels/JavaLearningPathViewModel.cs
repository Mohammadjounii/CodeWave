using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using CodeWave.Domain.Entities;

namespace CodeWave.Application.ViewModels
{
    public class JavaLearningPathViewModel
    {
        public Course Course { get; set; }
        public List<Lesson> Lessons { get; set; } = new();
        public HashSet<Guid> CompletedLessonIds { get; set; } = new();
        public Guid? CurrentLessonId { get; set; }

        public int CompletionPercent
        {
            get
            {
                if (Lessons == null || Lessons.Count == 0) return 0;
                return (int)Math.Round(100.0 * CompletedLessonIds.Count / Lessons.Count);
            }
        }
    }
}
