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

            var assessmentId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

            // Question GUIDs
            var q1  = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbb001");
            var q2  = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbb002");
            var q3  = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbb003");
            var q4  = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbb004");
            var q5  = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbb005");
            var q6  = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbb006");
            var q7  = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbb007");
            var q8  = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbb008");
            var q9  = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbb009");
            var q10 = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbb010");

            Guid A(int n) => Guid.Parse($"cccccccc-cccc-cccc-cccc-cccccccc{n:0000}");

            // -------------------------------------------------------
            // If the old 6-question set exists, replace it entirely.
            // Otherwise seed fresh if no assessment exists yet.
            // -------------------------------------------------------
            var existing = await context.Assessments
                .Include(a => a.Questions)
                    .ThenInclude(q => q.AnswerOptions)
                .FirstOrDefaultAsync(a => a.Id == assessmentId);

            bool needsReplacement = existing != null && existing.Questions.Count != 10;
            bool needsFreshSeed   = existing == null;

            if (!needsReplacement && !needsFreshSeed)
                return; // already up-to-date

            if (needsReplacement)
            {
                // Delete UserAnswers first (Restrict FK prevents cascade from Question)
                var oldQuestionIds    = existing.Questions.Select(q => q.Id).ToList();
                var oldOptionIds      = existing.Questions
                                               .SelectMany(q => q.AnswerOptions)
                                               .Select(o => o.Id)
                                               .ToList();

                var userAnswersToDelete = await context.UserAnswers
                    .Where(ua => oldQuestionIds.Contains(ua.QuestionId)
                              || oldOptionIds.Contains(ua.AnswerOptionId))
                    .ToListAsync();

                context.UserAnswers.RemoveRange(userAnswersToDelete);

                // Remove questions (cascades to AnswerOptions)
                context.Questions.RemoveRange(existing.Questions);
                await context.SaveChangesAsync();
            }

            if (needsFreshSeed)
            {
                var assessment = new Assessment
                {
                    Id    = assessmentId,
                    Title = "CodeWave Skill Assessment"
                };
                await context.Assessments.AddAsync(assessment);
            }

            // -------------------------------------------------------
            //  10 language-agnostic questions  (Beginner → Advanced)
            // -------------------------------------------------------
            var questions = new List<Question>
            {
                new Question { Id = q1,  AssessmentId = assessmentId, Text = "What does a compiler do?",                                                          Difficulty = "Beginner"     },
                new Question { Id = q2,  AssessmentId = assessmentId, Text = "What is the purpose of an 'if' statement in programming?",                          Difficulty = "Beginner"     },
                new Question { Id = q3,  AssessmentId = assessmentId, Text = "What is the difference between a function parameter and an argument?",               Difficulty = "Intermediate" },
                new Question { Id = q4,  AssessmentId = assessmentId, Text = "Which data structure uses Last-In, First-Out (LIFO) ordering?",                     Difficulty = "Intermediate" },
                new Question { Id = q5,  AssessmentId = assessmentId, Text = "What does it mean for a function to be recursive?",                                  Difficulty = "Intermediate" },
                new Question { Id = q6,  AssessmentId = assessmentId, Text = "Which OOP concept allows a subclass to provide its own version of a parent method?", Difficulty = "Intermediate" },
                new Question { Id = q7,  AssessmentId = assessmentId, Text = "What does the 'return' keyword do inside a function?",                               Difficulty = "Intermediate" },
                new Question { Id = q8,  AssessmentId = assessmentId, Text = "What is the average-case time complexity of a key lookup in a hash map?",            Difficulty = "Advanced"     },
                new Question { Id = q9,  AssessmentId = assessmentId, Text = "What does O(n log n) time complexity indicate about an algorithm?",                  Difficulty = "Advanced"     },
                new Question { Id = q10, AssessmentId = assessmentId, Text = "What is the primary purpose of an interface (or abstract class) in OOP?",            Difficulty = "Advanced"     },
            };

            var answers = new List<AnswerOption>
            {
                // Q1 — compiler
                new AnswerOption { Id = A(1),  QuestionId = q1,  Text = "Translates source code into machine-executable instructions",        IsCorrect = true  },
                new AnswerOption { Id = A(2),  QuestionId = q1,  Text = "Runs code one line at a time inside the terminal",                   IsCorrect = false },
                new AnswerOption { Id = A(3),  QuestionId = q1,  Text = "Automatically finds and fixes bugs in your code",                    IsCorrect = false },
                new AnswerOption { Id = A(4),  QuestionId = q1,  Text = "Manages how variables are stored at runtime",                        IsCorrect = false },

                // Q2 — if statement
                new AnswerOption { Id = A(5),  QuestionId = q2,  Text = "Executes a block of code only when a condition is true",             IsCorrect = true  },
                new AnswerOption { Id = A(6),  QuestionId = q2,  Text = "Repeats a block of code a fixed number of times",                   IsCorrect = false },
                new AnswerOption { Id = A(7),  QuestionId = q2,  Text = "Defines a reusable block of code with a name",                      IsCorrect = false },
                new AnswerOption { Id = A(8),  QuestionId = q2,  Text = "Imports an external library into the program",                      IsCorrect = false },

                // Q3 — parameter vs argument
                new AnswerOption { Id = A(9),  QuestionId = q3,  Text = "A parameter is the variable in the function definition; an argument is the value passed at the call site", IsCorrect = true  },
                new AnswerOption { Id = A(10), QuestionId = q3,  Text = "They are exactly the same thing — the terms are interchangeable",    IsCorrect = false },
                new AnswerOption { Id = A(11), QuestionId = q3,  Text = "A parameter is the return value; an argument is the input variable", IsCorrect = false },
                new AnswerOption { Id = A(12), QuestionId = q3,  Text = "Parameters belong to loops; arguments belong to functions",          IsCorrect = false },

                // Q4 — stack
                new AnswerOption { Id = A(13), QuestionId = q4,  Text = "Stack",                                                             IsCorrect = true  },
                new AnswerOption { Id = A(14), QuestionId = q4,  Text = "Queue",                                                             IsCorrect = false },
                new AnswerOption { Id = A(15), QuestionId = q4,  Text = "Linked List",                                                       IsCorrect = false },
                new AnswerOption { Id = A(16), QuestionId = q4,  Text = "Binary Tree",                                                       IsCorrect = false },

                // Q5 — recursion
                new AnswerOption { Id = A(17), QuestionId = q5,  Text = "The function calls itself within its own body",                     IsCorrect = true  },
                new AnswerOption { Id = A(18), QuestionId = q5,  Text = "The function runs on a separate background thread",                  IsCorrect = false },
                new AnswerOption { Id = A(19), QuestionId = q5,  Text = "The function can only accept a single argument",                    IsCorrect = false },
                new AnswerOption { Id = A(20), QuestionId = q5,  Text = "The function automatically retries on error",                       IsCorrect = false },

                // Q6 — polymorphism / method overriding
                new AnswerOption { Id = A(21), QuestionId = q6,  Text = "Polymorphism",                                                      IsCorrect = true  },
                new AnswerOption { Id = A(22), QuestionId = q6,  Text = "Encapsulation",                                                     IsCorrect = false },
                new AnswerOption { Id = A(23), QuestionId = q6,  Text = "Abstraction",                                                       IsCorrect = false },
                new AnswerOption { Id = A(24), QuestionId = q6,  Text = "Composition",                                                       IsCorrect = false },

                // Q7 — return keyword
                new AnswerOption { Id = A(25), QuestionId = q7,  Text = "Exits the function and optionally sends a value back to the caller", IsCorrect = true  },
                new AnswerOption { Id = A(26), QuestionId = q7,  Text = "Terminates the entire program immediately",                         IsCorrect = false },
                new AnswerOption { Id = A(27), QuestionId = q7,  Text = "Declares a new variable in the local scope",                        IsCorrect = false },
                new AnswerOption { Id = A(28), QuestionId = q7,  Text = "Restarts the function from the beginning",                          IsCorrect = false },

                // Q8 — hash map O(1)
                new AnswerOption { Id = A(29), QuestionId = q8,  Text = "O(1)",                                                              IsCorrect = true  },
                new AnswerOption { Id = A(30), QuestionId = q8,  Text = "O(n)",                                                              IsCorrect = false },
                new AnswerOption { Id = A(31), QuestionId = q8,  Text = "O(log n)",                                                          IsCorrect = false },
                new AnswerOption { Id = A(32), QuestionId = q8,  Text = "O(n²)",                                                             IsCorrect = false },

                // Q9 — O(n log n)
                new AnswerOption { Id = A(33), QuestionId = q9,  Text = "It scales slightly worse than linear — typical of efficient sorting algorithms like merge sort", IsCorrect = true  },
                new AnswerOption { Id = A(34), QuestionId = q9,  Text = "The algorithm always takes the same amount of time regardless of input size",                   IsCorrect = false },
                new AnswerOption { Id = A(35), QuestionId = q9,  Text = "The algorithm's runtime doubles every time the input size increases by one",                    IsCorrect = false },
                new AnswerOption { Id = A(36), QuestionId = q9,  Text = "It is the worst possible time complexity class",                                               IsCorrect = false },

                // Q10 — interface / abstract class
                new AnswerOption { Id = A(37), QuestionId = q10, Text = "To define a contract that implementing classes must fulfill",        IsCorrect = true  },
                new AnswerOption { Id = A(38), QuestionId = q10, Text = "To store shared data without any behavior",                         IsCorrect = false },
                new AnswerOption { Id = A(39), QuestionId = q10, Text = "To automatically generate documentation for a class",               IsCorrect = false },
                new AnswerOption { Id = A(40), QuestionId = q10, Text = "To prevent any other class from inheriting from it",                IsCorrect = false },
            };

            await context.Questions.AddRangeAsync(questions);
            await context.AnswerOptions.AddRangeAsync(answers);
            await context.SaveChangesAsync();
        }
    }
}
