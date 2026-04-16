-- Add Generated PDF Path Migration
-- Add field for storing generated professional PDF CV path

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CVs]') AND name = 'GeneratedPDFPath')
    ALTER TABLE [dbo].[CVs] ADD [GeneratedPDFPath] nvarchar(max) NULL;
GO




