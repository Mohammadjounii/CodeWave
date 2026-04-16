using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Data.Seed
{
    public static class SeedData
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

            // Prevent re-seeding
            if (context.Assessments.Any())
                return;

            // =============
            // FIXED GUIDs
            // =============

            var assessmentId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

            // Question GUIDs
            var q1 = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbb001");
            var q2 = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbb002");
            var q3 = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbb003");
            var q4 = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbb004");
            var q5 = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbb005");
            var q6 = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbb006");

            // AnswerOption GUIDs
            Guid A(int n) => Guid.Parse($"cccccccc-cccc-cccc-cccc-cccccccccc{n:00}");


            // =======================
            //     ASSESSMENT
            // =======================

            var assessment = new Assessment
            {
                Id = assessmentId,
                Title = "CodeWave Skill Assessment"
            };

            // =======================
            //     QUESTIONS
            // =======================

            var questions = new List<Question>
            {
                new Question { Id = q1, AssessmentId = assessmentId, Text = "What does HTML stand for?", Difficulty = "Beginner" },
                new Question { Id = q2, AssessmentId = assessmentId, Text = "What is the output of: print(2 * 3 + 1)?", Difficulty = "Beginner" },
                new Question { Id = q3, AssessmentId = assessmentId, Text = "Which OOP principle allows child classes to extend parent behavior?", Difficulty = "Intermediate" },
                new Question { Id = q4, AssessmentId = assessmentId, Text = "What does SQL stand for?", Difficulty = "Intermediate" },
                new Question { Id = q5, AssessmentId = assessmentId, Text = "What is Big O notation used for?", Difficulty = "Advanced" },
                new Question { Id = q6, AssessmentId = assessmentId, Text = "What is a REST API?", Difficulty = "Advanced" }
            };

            // =======================
            //   ANSWER OPTIONS
            // =======================

            var answers = new List<AnswerOption>
            {
                // Q1
                new AnswerOption { Id = A(1), QuestionId = q1, Text = "Hyper Text Markup Language", IsCorrect = true },
                new AnswerOption { Id = A(2), QuestionId = q1, Text = "Home Tool Markup Language", IsCorrect = false },
                new AnswerOption { Id = A(3), QuestionId = q1, Text = "Hyperlinks Text Media Language", IsCorrect = false },
                new AnswerOption { Id = A(4), QuestionId = q1, Text = "Hyper Text Making Language", IsCorrect = false },

                // Q2
                new AnswerOption { Id = A(5), QuestionId = q2, Text = "7", IsCorrect = true },
                new AnswerOption { Id = A(6), QuestionId = q2, Text = "5", IsCorrect = false },
                new AnswerOption { Id = A(7), QuestionId = q2, Text = "9", IsCorrect = false },
                new AnswerOption { Id = A(8), QuestionId = q2, Text = "Error", IsCorrect = false },

                // Q3
                new AnswerOption { Id = A(9), QuestionId = q3, Text = "Encapsulation", IsCorrect = false },
                new AnswerOption { Id = A(10), QuestionId = q3, Text = "Inheritance", IsCorrect = true },
                new AnswerOption { Id = A(11), QuestionId = q3, Text = "Abstraction", IsCorrect = false },
                new AnswerOption { Id = A(12), QuestionId = q3, Text = "Polymorphism", IsCorrect = false },

                // Q4
                new AnswerOption { Id = A(13), QuestionId = q4, Text = "Structured Query Language", IsCorrect = true },
                new AnswerOption { Id = A(14), QuestionId = q4, Text = "Sequential Query Logic", IsCorrect = false },
                new AnswerOption { Id = A(15), QuestionId = q4, Text = "Simple Query Language", IsCorrect = false },
                new AnswerOption { Id = A(16), QuestionId = q4, Text = "Standard Query Layout", IsCorrect = false },

                // Q5
                new AnswerOption { Id = A(17), QuestionId = q5, Text = "Algorithm time complexity", IsCorrect = true },
                new AnswerOption { Id = A(18), QuestionId = q5, Text = "Data storage format", IsCorrect = false },
                new AnswerOption { Id = A(19), QuestionId = q5, Text = "Network capacity model", IsCorrect = false },
                new AnswerOption { Id = A(20), QuestionId = q5, Text = "Binary operation mode", IsCorrect = false },

                // Q6
                new AnswerOption { Id = A(21), QuestionId = q6, Text = "Stateless web architecture style", IsCorrect = true },
                new AnswerOption { Id = A(22), QuestionId = q6, Text = "A testing protocol", IsCorrect = false },
                new AnswerOption { Id = A(23), QuestionId = q6, Text = "A database model", IsCorrect = false },
                new AnswerOption { Id = A(24), QuestionId = q6, Text = "A debugging tool", IsCorrect = false }
            };


            // =======================
            // ADD TO DB
            // =======================

            await context.Assessments.AddAsync(assessment);
            await context.Questions.AddRangeAsync(questions);
            await context.AnswerOptions.AddRangeAsync(answers);

            await context.SaveChangesAsync();
        }
    }
}
