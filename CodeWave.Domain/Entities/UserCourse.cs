using System;
using System.Collections.Generic;
using System.Text;

namespace CodeWave.Domain.Entities
{
    public class UserCourse
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public Guid CourseId { get; set; }

        public int ProgressPercent { get; set; }
        public DateTime? CompletionDate { get; set; }

        public ApplicationUser User { get; set; }
        public Course Course { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool isDeleted { get; set; } = false;
    }
}
