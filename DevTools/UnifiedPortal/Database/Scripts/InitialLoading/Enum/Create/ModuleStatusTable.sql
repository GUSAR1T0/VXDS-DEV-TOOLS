CREATE TABLE [enum].[ModuleStatus]
(
    [Id]   TINYINT IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR(50)            NOT NULL,
    CONSTRAINT [PK_ModuleStatus_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);