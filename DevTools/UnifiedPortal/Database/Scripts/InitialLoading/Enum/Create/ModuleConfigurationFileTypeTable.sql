CREATE TABLE [enum].[ModuleConfigurationFileType]
(
    [Id]   TINYINT IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR(16)            NOT NULL,
    CONSTRAINT [PK_ModuleConfigurationFileType_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);