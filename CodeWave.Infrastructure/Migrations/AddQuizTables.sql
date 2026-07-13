-- Add Quiz Tables Migration
-- Run this script manually on your database if the Quiz tables don't exist

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Quizzes]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Quizzes] (
        [Id] uniqueidentifier NOT NULL,
        [CourseId] uniqueidentifier NOT NULL,
        [Title] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [TimeLimitMinutes] int NOT NULL,
        [PassingScore] int NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_Quizzes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Quizzes_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [dbo].[Courses] ([Id]) ON DELETE CASCADE
    );
    
    CREATE INDEX [IX_Quizzes_CourseId] ON [dbo].[Quizzes] ([CourseId]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QuizQuestions]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[QuizQuestions] (
        [Id] uniqueidentifier NOT NULL,
        [QuizId] uniqueidentifier NOT NULL,
        [Text] nvarchar(max) NOT NULL,
        [Difficulty] nvarchar(max) NOT NULL,
        [OrderNumber] int NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_QuizQuestions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_QuizQuestions_Quizzes_QuizId] FOREIGN KEY ([QuizId]) REFERENCES [dbo].[Quizzes] ([Id]) ON DELETE CASCADE
    );
    
    CREATE INDEX [IX_QuizQuestions_QuizId] ON [dbo].[QuizQuestions] ([QuizId]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[QuizAnswerOptions]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[QuizAnswerOptions] (
        [Id] uniqueidentifier NOT NULL,
        [QuizQuestionId] uniqueidentifier NOT NULL,
        [Text] nvarchar(max) NOT NULL,
        [IsCorrect] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_QuizAnswerOptions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_QuizAnswerOptions_QuizQuestions_QuizQuestionId] FOREIGN KEY ([QuizQuestionId]) REFERENCES [dbo].[QuizQuestions] ([Id]) ON DELETE CASCADE
    );
    
    CREATE INDEX [IX_QuizAnswerOptions_QuizQuestionId] ON [dbo].[QuizAnswerOptions] ([QuizQuestionId]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserQuizAttempts]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[UserQuizAttempts] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [QuizId] uniqueidentifier NOT NULL,
        [Score] float NOT NULL,
        [IsPassed] bit NOT NULL,
        [StartedAt] datetime2 NOT NULL,
        [CompletedAt] datetime2 NULL,
        [TimeSpentMinutes] int NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_UserQuizAttempts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_UserQuizAttempts_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserQuizAttempts_Quizzes_QuizId] FOREIGN KEY ([QuizId]) REFERENCES [dbo].[Quizzes] ([Id]) ON DELETE NO ACTION
    );
    
    CREATE INDEX [IX_UserQuizAttempts_UserId] ON [dbo].[UserQuizAttempts] ([UserId]);
    CREATE INDEX [IX_UserQuizAttempts_QuizId] ON [dbo].[UserQuizAttempts] ([QuizId]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserQuizAnswers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[UserQuizAnswers] (
        [Id] uniqueidentifier NOT NULL,
        [UserQuizAttemptId] uniqueidentifier NOT NULL,
        [QuizQuestionId] uniqueidentifier NOT NULL,
        [SelectedAnswerOptionId] uniqueidentifier NULL,
        [IsCorrect] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [IsDeleted] bit NOT NULL,
        CONSTRAINT [PK_UserQuizAnswers] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_UserQuizAnswers_UserQuizAttempts_UserQuizAttemptId] FOREIGN KEY ([UserQuizAttemptId]) REFERENCES [dbo].[UserQuizAttempts] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserQuizAnswers_QuizQuestions_QuizQuestionId] FOREIGN KEY ([QuizQuestionId]) REFERENCES [dbo].[QuizQuestions] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_UserQuizAnswers_QuizAnswerOptions_SelectedAnswerOptionId] FOREIGN KEY ([SelectedAnswerOptionId]) REFERENCES [dbo].[QuizAnswerOptions] ([Id]) ON DELETE NO ACTION
    );
    
    CREATE INDEX [IX_UserQuizAnswers_UserQuizAttemptId] ON [dbo].[UserQuizAnswers] ([UserQuizAttemptId]);
    CREATE INDEX [IX_UserQuizAnswers_QuizQuestionId] ON [dbo].[UserQuizAnswers] ([QuizQuestionId]);
    CREATE INDEX [IX_UserQuizAnswers_SelectedAnswerOptionId] ON [dbo].[UserQuizAnswers] ([SelectedAnswerOptionId]);
END
GO

