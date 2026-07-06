using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeWave.Infrastructure.Data.Seed;

public static class AchievementSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Create tables if they don't exist (avoids needing a new EF migration)
        await context.Database.ExecuteSqlRawAsync(@"
            IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Achievements')
            BEGIN
                CREATE TABLE Achievements (
                    Id           UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
                    [Key]        NVARCHAR(100)    NOT NULL,
                    Name         NVARCHAR(100)    NOT NULL,
                    Description  NVARCHAR(300)    NOT NULL,
                    Icon         NVARCHAR(10)     NOT NULL,
                    GradientFrom NVARCHAR(20)     NOT NULL,
                    GradientTo   NVARCHAR(20)     NOT NULL,
                    Category     NVARCHAR(50)     NOT NULL,
                    XPValue      INT              NOT NULL,
                    CONSTRAINT PK_Achievements PRIMARY KEY (Id),
                    CONSTRAINT UQ_Achievements_Key UNIQUE ([Key])
                );
            END");

        await context.Database.ExecuteSqlRawAsync(@"
            IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'UserAchievements')
            BEGIN
                CREATE TABLE UserAchievements (
                    Id            UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
                    UserId        UNIQUEIDENTIFIER NOT NULL,
                    AchievementId UNIQUEIDENTIFIER NOT NULL,
                    EarnedAt      DATETIME2        NOT NULL,
                    CONSTRAINT PK_UserAchievements PRIMARY KEY (Id),
                    CONSTRAINT UQ_UserAchievements_User_Achievement UNIQUE (UserId, AchievementId),
                    CONSTRAINT FK_UserAchievements_Achievements FOREIGN KEY (AchievementId)
                        REFERENCES Achievements(Id) ON DELETE CASCADE,
                    CONSTRAINT FK_UserAchievements_Users FOREIGN KEY (UserId)
                        REFERENCES AspNetUsers(Id) ON DELETE CASCADE
                );
                CREATE INDEX IX_UserAchievements_AchievementId ON UserAchievements(AchievementId);
            END");

        var definitions = new List<(string Key, string Name, string Desc, string Icon, string From, string To, string Cat, int XP)>
        {
            // ── Learning ──────────────────────────────────────────────────
            ("first_lesson",   "First Steps",        "Complete your very first lesson",               "🎯", "#3b82f6", "#6366f1", "Learning",   50),
            ("lesson_5",       "On a Roll",          "Complete 5 lessons",                            "🔥", "#f59e0b", "#ef4444", "Learning",  100),
            ("lesson_10",      "Dedicated Learner",  "Complete 10 lessons",                           "📚", "#10b981", "#3b82f6", "Learning",  200),
            ("lesson_25",      "Knowledge Seeker",   "Complete 25 lessons",                           "🧠", "#8b5cf6", "#ec4899", "Learning",  400),
            ("course_complete","Course Champion",    "Finish every lesson in a full course",          "🏆", "#f59e0b", "#ef4444", "Learning",  500),
            ("first_exercise", "Code Initiate",      "Solve your first coding exercise",              "💻", "#06b6d4", "#3b82f6", "Learning",   75),
            ("exercise_10",    "Problem Solver",     "Solve 10 coding exercises correctly",           "⚡", "#8b5cf6", "#06b6d4", "Learning",  250),
            // ── Quizzes ───────────────────────────────────────────────────
            ("first_quiz",     "Quiz Rookie",        "Pass your first quiz",                          "📝", "#6366f1", "#8b5cf6", "Quizzes",   100),
            ("quiz_3",         "Quiz Enthusiast",    "Pass 3 quizzes",                               "🌟", "#8b5cf6", "#ec4899", "Quizzes",   200),
            ("quiz_master",    "Quiz Master",        "Score 100% on any quiz",                        "🎯", "#f59e0b", "#f97316", "Quizzes",   300),
            // ── Career ────────────────────────────────────────────────────
            ("cv_created",     "Resume Ready",       "Create and save your CV",                       "📄", "#10b981", "#06b6d4", "Career",    150),
            ("first_job_app",  "Job Hunter",         "Submit your first job application",             "💼", "#f59e0b", "#10b981", "Career",    200),
            // ── Milestones ────────────────────────────────────────────────
            ("assessment_done","Self-Aware",         "Complete the skill assessment",                 "🎓", "#6366f1", "#8b5cf6", "Milestones", 75),
            ("streak_3",       "3-Day Streak",       "Study at least one lesson every day for 3 days","🔥", "#ef4444", "#f59e0b", "Milestones",150),
            ("playground_use", "Tinkerer",           "Write and run code in the Playground",          "🔬", "#06b6d4", "#10b981", "Milestones", 25),
        };

        foreach (var (key, name, desc, icon, from, to, cat, xp) in definitions)
        {
            if (!await context.Achievements.AnyAsync(a => a.Key == key))
            {
                context.Achievements.Add(new Achievement
                {
                    Id           = Guid.NewGuid(),
                    Key          = key,
                    Name         = name,
                    Description  = desc,
                    Icon         = icon,
                    GradientFrom = from,
                    GradientTo   = to,
                    Category     = cat,
                    XPValue      = xp
                });
            }
        }

        await context.SaveChangesAsync();
    }
}
