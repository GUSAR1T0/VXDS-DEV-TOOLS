CREATE TABLE [portal].[ModuleHistory]
(
    [Id]       INT IDENTITY (1, 1)                NOT NULL,
    [ModuleId] INT                                NOT NULL,
    [Time]     DATETIME2(3) DEFAULT SYSDATETIME() NOT NULL,
    [Change]   NVARCHAR(MAX)                      NOT NULL,
    CONSTRAINT [PK_ModuleHistory_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_ModuleHistory_ModuleId] FOREIGN KEY ([ModuleId]) REFERENCES [portal].[Module] ([Id]) ON DELETE CASCADE
);