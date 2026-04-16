using System;
using System.Collections.Generic;
using System.Text;

namespace CodeWave.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Result { get; set; }

        public ApplicationUser User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool isDeleted { get; set; } = false;
    }
}
