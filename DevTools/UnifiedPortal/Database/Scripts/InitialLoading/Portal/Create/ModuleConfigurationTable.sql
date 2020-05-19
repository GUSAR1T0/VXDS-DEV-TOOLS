CREATE TABLE [portal].[ModuleConfiguration]
(
    [Id]       INT IDENTITY (1, 1) NOT NULL,
    [ModuleId] INT                 NOT NULL,
    [Name]     NVARCHAR(255)       NOT NULL,
    [Version]  NVARCHAR(50)        NOT NULL,
    [Author]   NVARCHAR(100)       NOT NULL,
    [Email]    NVARCHAR(255)       NOT NULL,
    [FileId]   INT                 NOT NULL,
    CONSTRAINT [PK_ModuleConfiguration_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ModuleConfiguration_ModuleId] FOREIGN KEY ([ModuleId]) REFERENCES [portal].[Module] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ModuleConfiguration_FileId] FOREIGN KEY ([FileId]) REFERENCES [base].[File] ([Id]) ON DELETE CASCADE
);