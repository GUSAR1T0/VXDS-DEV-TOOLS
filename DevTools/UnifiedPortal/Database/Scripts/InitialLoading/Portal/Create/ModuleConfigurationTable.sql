CREATE TABLE [portal].[ModuleConfiguration]
(
    [Id]         INT IDENTITY (1, 1)                  NOT NULL,
    [ModuleId]   INT                                  NOT NULL,
    [Name]       NVARCHAR(255)                        NOT NULL,
    [Version]    NVARCHAR(50)                         NOT NULL,
    [Author]     NVARCHAR(100)                        NOT NULL,
    [Email]      NVARCHAR(255)                        NOT NULL,
    [FileName]   NVARCHAR(512)                        NOT NULL,
    [FileTypeId] TINYINT                              NOT NULL,
    [Content]    VARBINARY(MAX)                       NOT NULL,
    [Time]       DATETIME2(3) DEFAULT (SYSDATETIME()) NOT NULL,
    CONSTRAINT [PK_ModuleConfiguration_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ModuleConfiguration_ModuleId] FOREIGN KEY ([ModuleId]) REFERENCES [portal].[Module] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ModuleConfiguration_FileTypeId] FOREIGN KEY ([FileTypeId]) REFERENCES [enum].[ModuleConfigurationFileType] ([Id]) ON DELETE CASCADE
);