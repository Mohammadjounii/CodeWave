using System;
using System.Collections.Generic;
using System.Text;

namespace CodeWave.Domain.Entities
{
    public class JobOffer
    {
        public Guid Id { get; set; }

        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public string RequiredSkills { get; set; }

        public DateTime PostedDate { get; set; }
        public DateTime Deadline { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool isDeleted { get; set; } = false;
    }
}
