CREATE TABLE [enum].[FileExtension]
(
    [Id]   TINYINT IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR(16)            NOT NULL,
    CONSTRAINT [PK_FileExtension_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);