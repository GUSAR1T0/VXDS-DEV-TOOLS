CREATE TABLE [enum].[NotificationLevel]
(
    [Id]   TINYINT IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR(255)           NOT NULL,
    CONSTRAINT [PK_NotificationLevel_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);