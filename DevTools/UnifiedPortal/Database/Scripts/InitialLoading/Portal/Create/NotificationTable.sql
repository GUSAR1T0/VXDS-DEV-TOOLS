CREATE TABLE [portal].[Notification]
(
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [Message]   NVARCHAR(1024)      NOT NULL,
    [LevelId]   TINYINT             NOT NULL,
    [StartTime] DATETIME2(3)        NOT NULL,
    [StopTime]  DATETIME2(3)        NOT NULL,
    [UserId]    INT                 NULL,
    CONSTRAINT [PK_Notification_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Notification_LevelId] FOREIGN KEY ([LevelId]) REFERENCES [enum].[NotificationLevel] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Notification_UserId] FOREIGN KEY ([UserId]) REFERENCES [authentication].[User] ([Id])
);