using System;
using System.Collections.Generic;
using System.Text;

namespace CodeWave.Domain.Entities
{
    public class Assessment
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public ICollection<Question> Questions { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool isDeleted { get; set; } = false;
    }
}
