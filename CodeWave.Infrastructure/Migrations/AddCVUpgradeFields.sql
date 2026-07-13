-- Add CV Upgrade Fields Migration
-- Add fields for CV file upload and upgrade functionality

-- Uploaded CV File Path
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CVs]') AND name = 'UploadedCVFilePath')
    ALTER TABLE [dbo].[CVs] ADD [UploadedCVFilePath] nvarchar(max) NULL;
GO

-- Upgraded CV File Path
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[CVs]') AND name = 'UpgradedCVFilePath')
    ALTER TABLE [dbo].[CVs] ADD [UpgradedCVFilePath] nvarchar(max) NULL;
GO




