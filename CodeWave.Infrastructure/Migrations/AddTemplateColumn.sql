-- Add Template Column Migration
-- Add field for storing selected CV template

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CVs]') AND name = 'Template')
BEGIN
    ALTER TABLE [dbo].[CVs] ADD [Template] nvarchar(max) NULL;
END

