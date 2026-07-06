using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CodeWave.Domain.Entities;

namespace CodeWave.Web.Models
{
    public class CourseWithLessonsViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string? DifficultyLevel { get; set; }

        [Required]
        public string LearningPath { get; set; } = string.Empty;

        public Language? ProgrammingLanguage { get; set; } = Language.Java;

        public List<LessonViewModel> Lessons { get; set; } = new List<LessonViewModel>();
    }

    public class LessonViewModel
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public string? VideoUrl { get; set; }
        public string? ImageUrl { get; set; }

        public int OrderNumber { get; set; }

        public bool IsDeleted { get; set; }
    }
}

