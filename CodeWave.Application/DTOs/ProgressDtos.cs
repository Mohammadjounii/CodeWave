using System;
using System.Collections.Generic;

namespace CodeWave.Application.DTOs
{
    public class UserProgressDto
    {
        public int TotalLessons { get; set; }
        public int CompletedLessons { get; set; }
        public int TotalExercises { get; set; }
        public int CompletedExercises { get; set; }
        public int TotalQuizzes { get; set; }
        public int PassedQuizzes { get; set; }
        public double OverallProgressPercent { get; set; }
        public int TotalStudyTimeMinutes { get; set; }
        public string LearningPath { get; set; }
    }

    public class SkillProgressDto
    {
        public string SkillName { get; set; }
        public int MasteryLevel { get; set; } // 0-100
        public int LessonsCompleted { get; set; }
        public int ExercisesCompleted { get; set; }
        public bool IsMastered { get; set; }
    }

    public class WeaknessDto
    {
        public string Topic { get; set; }
        public string Description { get; set; }
        public int FailedAttempts { get; set; }
        public double AverageScore { get; set; }
        public List<string> RecommendedLessons { get; set; } = new List<string>();
    }
}

