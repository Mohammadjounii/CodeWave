using CodeWave.Domain.Entities;
using CodeWave.Infrastructure.Data.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection.Emit;


namespace CodeWave.Infrastructure.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // =======================
        //   IDENTITY TABLES
        // =======================
        // (Generated automatically by IdentityDbContext)

        // =======================
        //   ASSESSMENT
        // =======================
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<UserAssesment> UserAssessments { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }

        // =======================
        //   LEARNING CONTENT
        // =======================
        public DbSet<Course> Courses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<CodingExercise> CodingExercises { get; set; }
        public DbSet<ExerciseTestCase> ExerciseTestCases { get; set; }

        // =======================
        //   USER PROGRESS
        // =======================
        public DbSet<ExerciseSubmission> ExerciseSubmissions { get; set; }
        public DbSet<LessonCompletion> LessonCompletions { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }

        // =======================
        //   PROJECTS + JOB OFFERS
        // =======================
        public DbSet<Project> Projects { get; set; }
        public DbSet<JobOffer> JobOffers { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<CV> CVs { get; set; }

        // =======================
        //   QUIZ SYSTEM
        // =======================
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<QuizAnswerOption> QuizAnswerOptions { get; set; }
        public DbSet<UserQuizAttempt> UserQuizAttempts { get; set; }
        public DbSet<UserQuizAnswer> UserQuizAnswers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            
            // Suppress the pending model changes warning
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ==============================
            // APPLICATION USER CONFIGURATION
            // ==============================
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.Description).IsRequired(false);
                entity.Property(e => e.Level).IsRequired(false);
                entity.Property(e => e.LearningPath).IsRequired(false);
                entity.Property(e => e.LastName).IsRequired(false);
                entity.Property(e => e.Interests).IsRequired(false);
                entity.Property(e => e.Goal).IsRequired(false);
                entity.Property(e => e.SkillLevel).IsRequired(false);
                entity.Property(e => e.Motivation).IsRequired(false);
                entity.Property(e => e.WeeklyHours).IsRequired(false);
                entity.Property(e => e.PreferredLanguage).IsRequired(false);
                entity.Property(e => e.ProfilePictureUrl).IsRequired(false);
            });

            // ==============================
            // CV CONFIGURATION
            // ==============================
            builder.Entity<CV>(entity =>
            {
                entity.Property(e => e.FullName).IsRequired(false);
                entity.Property(e => e.Age).IsRequired(false);
                entity.Property(e => e.Location).IsRequired(false);
                entity.Property(e => e.Email).IsRequired(false);
                entity.Property(e => e.Phone).IsRequired(false);
                entity.Property(e => e.LinkedInUrl).IsRequired(false);
                entity.Property(e => e.GitHubUrl).IsRequired(false);
                entity.Property(e => e.CVPictureUrl).IsRequired(false);
                entity.Property(e => e.UploadedCVFilePath).IsRequired(false);
                entity.Property(e => e.UpgradedCVFilePath).IsRequired(false);
                entity.Property(e => e.GeneratedPDFPath).IsRequired(false);
                entity.Property(e => e.Education).IsRequired(false);
                entity.Property(e => e.EducationDetails).IsRequired(false);
                entity.Property(e => e.Skills).IsRequired(false);
                entity.Property(e => e.ProgrammingLanguages).IsRequired(false);
                entity.Property(e => e.SpokenLanguages).IsRequired(false);
                entity.Property(e => e.Summary).IsRequired(false);
                entity.Property(e => e.Experience).IsRequired(false);
                entity.Property(e => e.Certifications).IsRequired(false);
                entity.Property(e => e.Projects).IsRequired(false);
                entity.Property(e => e.Template).IsRequired(false);

                entity.HasOne(c => c.User)
                    .WithOne(u => u.CV)
                    .HasForeignKey<CV>(c => c.UserId);

                entity.HasIndex(c => c.UserId).IsUnique();
            });

            // ==============================
            // JOB APPLICATION CONFIGURATION
            // ==============================
            builder.Entity<JobApplication>(entity =>
            {
                entity.Property(e => e.Status).IsRequired(false);
                entity.Property(e => e.CoverLetter).IsRequired(false);
                entity.Property(e => e.MatchPercentage).HasDefaultValue(0.0);
                
                entity.HasOne(ja => ja.User)
                    .WithMany()
                    .HasForeignKey(ja => ja.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasOne(ja => ja.JobOffer)
                    .WithMany()
                    .HasForeignKey(ja => ja.JobOfferId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ==============================
            // QUESTION -> ASSESSMENT
            // ==============================
            builder.Entity<Question>()
                .HasOne(q => q.Assessment)
                .WithMany(a => a.Questions)
                .HasForeignKey(q => q.AssessmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // ==============================
            // ANSWEROPTION -> QUESTION
            // ==============================
            builder.Entity<AnswerOption>()
                .HasOne(a => a.Question)
                .WithMany(q => q.AnswerOptions)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // ==============================
            // USERANSWER -> MULTIPLE ENTITIES
            // ==============================
            builder.Entity<UserAnswer>()
                .HasOne(ua => ua.Question)
                .WithMany(q => q.UserAnswers)
                .HasForeignKey(ua => ua.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==============================
            // COURSE CONFIGURATION
            // ==============================
            builder.Entity<Course>(entity =>
            {
                entity.Property(e => e.ProgrammingLanguage)
                    .HasConversion<int>() // Store enum as int in database
                    .IsRequired(false);
            });

            builder.Entity<UserAnswer>()
                .HasOne(ua => ua.AnswerOption)
                .WithMany(a => a.UserAnswers)
                .HasForeignKey(ua => ua.AnswerOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserAnswer>()
                .HasOne(ua => ua.UserAssessment)
                .WithMany(ua2 => ua2.UserAnswers)
                .HasForeignKey(ua => ua.UserAssessmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // ==============================
            // QUIZ SYSTEM RELATIONSHIPS
            // ==============================
            builder.Entity<Quiz>()
                .HasOne(q => q.Course)
                .WithMany(c => c.Quizzes)
                .HasForeignKey(q => q.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<QuizQuestion>()
                .HasOne(qq => qq.Quiz)
                .WithMany(q => q.Questions)
                .HasForeignKey(qq => qq.QuizId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<QuizAnswerOption>()
                .HasOne(qao => qao.QuizQuestion)
                .WithMany(qq => qq.AnswerOptions)
                .HasForeignKey(qao => qao.QuizQuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserQuizAttempt>()
                .HasOne(uqa => uqa.User)
                .WithMany(u => u.UserQuizAttempts)
                .HasForeignKey(uqa => uqa.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserQuizAttempt>()
                .HasOne(uqa => uqa.Quiz)
                .WithMany(q => q.UserQuizAttempts)
                .HasForeignKey(uqa => uqa.QuizId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserQuizAnswer>()
                .HasOne(uqa => uqa.UserQuizAttempt)
                .WithMany(uqat => uqat.UserQuizAnswers)
                .HasForeignKey(uqa => uqa.UserQuizAttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserQuizAnswer>()
                .HasOne(uqa => uqa.QuizQuestion)
                .WithMany()
                .HasForeignKey(uqa => uqa.QuizQuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserQuizAnswer>()
                .HasOne(uqa => uqa.SelectedAnswerOption)
                .WithMany()
                .HasForeignKey(uqa => uqa.SelectedAnswerOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            // ExerciseTestCase -> CodingExercise relationship
            builder.Entity<ExerciseTestCase>()
                .HasOne(etc => etc.Exercise)
                .WithMany(e => e.TestCases)
                .HasForeignKey(etc => etc.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            Guid javaCourseId = new Guid("11111111-1111-1111-1111-111111111111");

            // 1️⃣ Seed the Course first
            builder.Entity<Course>().HasData(
                new Course
                {
                    Id = javaCourseId,
                    Title = "Java Programming",
                    Description = "Learn Java from basics to advanced",
                    CreatedAt = new DateTime(2025, 12, 4),
                    IsDeleted = false
                }
            );

            // 2️⃣ Seed Lessons for the Java course
            builder.Entity<Lesson>().HasData(
                new Lesson
                {
                    Id = new Guid("a1111111-1111-1111-1111-111111111111"),
                    CourseId = javaCourseId,
                    Title = "Introduction to Java",
                    OrderNumber = 1,
                    Content = "Java is a high-level, object-oriented programming language used on billions of devices. It is platform-independent thanks to the JVM.",
                    ImageUrl = "https://via.placeholder.com/150",   
                    VideoUrl = "https://www.example.com/java-intro-video",
                    CreatedAt = new DateTime(2025, 12, 4),
                    isDeleted = false
                },
                new Lesson
                {
                    Id = new Guid("a2222222-2222-2222-2222-222222222222"),
                    CourseId = javaCourseId,
                    Title = "Java Syntax and First Program",
                    OrderNumber = 2,
                    Content = "A Java program must contain a class and a main method.\nExample:\npublic class Main { public static void main(String[] args) { System.out.println(\"Hello World\"); } }",
                    ImageUrl = "https://via.placeholder.com/150",
                    VideoUrl = "https://www.example.com/java-syntax-video", 
                    CreatedAt = new DateTime(2025, 12, 4),
                    isDeleted = false
                },
                new Lesson
                {
                    Id = new Guid("a3333333-3333-3333-3333-333333333333"),
                    CourseId = javaCourseId,
                    Title = "Java Variables",
                    OrderNumber = 3,
                    Content = "Variables store data values. Java has different types such as int, double, char, String.\nExample:\nint age = 20;\ndouble price = 10.99;\nString name = \"Alice\";",
                    ImageUrl = "https://via.placeholder.com/150",
                    VideoUrl = "https://www.example.com/java-variables-video",
                    CreatedAt = new DateTime(2025, 12, 4),
                    isDeleted = false
                }
            // …add the rest of your lessons here
            );
        }

        public override int SaveChanges()
        {
            ValidateQuizQuestions();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ValidateQuizQuestions();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ValidateQuizQuestions()
        {
            // Only validate quizzes that are being modified (not newly added - allow new quizzes without questions initially)
            // This allows seed data to create quizzes first, then add questions
            var quizzesToValidate = ChangeTracker.Entries<Quiz>()
                .Where(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Modified &&
                           !e.Entity.IsDeleted)
                .Select(e => e.Entity.Id)
                .ToList();

            if (quizzesToValidate.Any())
            {
                // For each quiz being modified, check if it has at least one non-deleted question
                foreach (var quizId in quizzesToValidate)
                {
                    // Count questions in the database and in the change tracker
                    var existingQuestions = QuizQuestions
                        .Where(qq => qq.QuizId == quizId && !qq.IsDeleted)
                        .Count();

                    var newQuestions = ChangeTracker.Entries<QuizQuestion>()
                        .Where(e => e.Entity.QuizId == quizId && 
                                   !e.Entity.IsDeleted &&
                                   (e.State == Microsoft.EntityFrameworkCore.EntityState.Added || 
                                    e.State == Microsoft.EntityFrameworkCore.EntityState.Modified))
                        .Count();

                    var deletedQuestions = ChangeTracker.Entries<QuizQuestion>()
                        .Where(e => e.Entity.QuizId == quizId && 
                                   e.State == Microsoft.EntityFrameworkCore.EntityState.Deleted)
                        .Count();

                    var totalQuestions = existingQuestions + newQuestions - deletedQuestions;

                    // Only validate if quiz is being modified and would end up with no questions
                    if (totalQuestions == 0)
                    {
                        var quiz = ChangeTracker.Entries<Quiz>()
                            .FirstOrDefault(e => e.Entity.Id == quizId)?.Entity;
                        var quizTitle = quiz?.Title ?? "Unknown";
                        throw new InvalidOperationException(
                            $"Quiz '{quizTitle}' must have at least one question. " +
                            "Please add questions before saving the quiz.");
                    }
                }
            }
        }
    }
}
