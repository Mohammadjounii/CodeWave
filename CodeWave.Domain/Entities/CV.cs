using System;
using System.Collections.Generic;
using System.Text;

namespace CodeWave.Domain.Entities
{
    public class CV
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        // Personal Information
        public string FullName { get; set; }
        public int? Age { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LinkedInUrl { get; set; }
        public string GitHubUrl { get; set; }
        public string CVPictureUrl { get; set; }  // CV-specific picture
        public string UploadedCVFilePath { get; set; }  // Path to uploaded CV file (PDF/DOCX)
        public string UpgradedCVFilePath { get; set; }  // Path to upgraded professional CV file
        public string GeneratedPDFPath { get; set; }  // Path to generated professional PDF CV
        public string Template { get; set; } = "modern";  // CV template name (modern, classic, creative, minimalist, executive)

        // Education
        public string Education { get; set; }        // Where they studied
        public string EducationDetails { get; set; } // Degree, field of study, etc.

        // Skills
        public string Skills { get; set; }                 // General skills (optional, legacy field)
        public string ProgrammingLanguages { get; set; }  // HTML, CSS, Java, Python, etc. (comma-separated or JSON)
        public string SpokenLanguages { get; set; }        // English, Arabic, etc. (comma-separated or JSON)

        // Professional Information
        public string Summary { get; set; }          // Short professional summary
        public string Experience { get; set; }       // Work experience (can be JSON or formatted text)
        public string Certifications { get; set; }   // Certificates or achievements
        public string Projects { get; set; }         // Projects section

        public DateTime LastUpdated { get; set; }
        public DateTime CreatedAt { get; set; }

        public ApplicationUser User { get; set; }
    }
}
