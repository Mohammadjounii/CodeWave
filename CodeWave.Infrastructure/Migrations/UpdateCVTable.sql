-- Update CV Table Migration
-- Add new columns to the CV table for the enhanced CV builder
-- Run this script manually on your database

-- Personal Information
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CVs]') AND name = 'FullName')
    ALTER TABLE [dbo].[CVs] ADD [FullName] nvarchar(max) NULL;
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CVs]') AND name = 'Age')
    ALTER TABLE [dbo].[CVs] ADD [Age] int NULL;
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CVs]') AND name = 'Location')
    ALTER TABLE [dbo].[CVs] ADD [Location] nvarchar(max) NULL;
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CVs]') AND name = 'Email')
    ALTER TABLE [dbo].[CVs] ADD [Email] nvarchar(max) NULL;
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CVs]') AND name = 'Phone')
    ALTER TABLE [dbo].[CVs] ADD [Phone] nvarchar(max) NULL;
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CVs]') AND name = 'LinkedInUrl')
    ALTER TABLE [dbo].[CVs] ADD [LinkedInUrl] nvarchar(max) NULL;
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CVs]') AND name = 'GitHubUrl')
    ALTER TABLE [dbo].[CVs] ADD [GitHubUrl] nvarchar(max) NULL;
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CVs]') AND name = 'CVPictureUrl')
    ALTER TABLE [dbo].[CVs] ADD [CVPictureUrl] nvarchar(max) NULL;
GO

-- Education
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CVs]') AND name = 'EducationDetails')
    ALTER TABLE [dbo].[CVs] ADD [EducationDetails] nvarchar(max) NULL;
GO

-- Skills
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CVs]') AND name = 'ProgrammingLanguages')
    ALTER TABLE [dbo].[CVs] ADD [ProgrammingLanguages] nvarchar(max) NULL;
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CVs]') AND name = 'SpokenLanguages')
    ALTER TABLE [dbo].[CVs] ADD [SpokenLanguages] nvarchar(max) NULL;
GO

-- Projects
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CVs]') AND name = 'Projects')
    ALTER TABLE [dbo].[CVs] ADD [Projects] nvarchar(max) NULL;
GO

-- Timestamps
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CVs]') AND name = 'CreatedAt')
BEGIN
    ALTER TABLE [dbo].[CVs] ADD [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE();
END
GO

