using System;
using System.Collections.Generic;
using System.Text;

namespace CodeWave.Domain.Entities
{
    public class LessonCompletion
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public Guid LessonId { get; set; }

        public bool IsCompleted { get; set; }
        public DateTime? CompletionDate { get; set; }
        public int TimeSpentMinutes { get; set; } = 0; // Time spent on this lesson in minutes

        public ApplicationUser User { get; set; }
        public Lesson Lesson { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool isDeleted { get; set; } = false;
    }
}
