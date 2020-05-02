CREATE TABLE [enum].[OperatingSystem]
(
    [Id]   TINYINT IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR(255)           NOT NULL,
    CONSTRAINT [PK_OperatingSystem_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);