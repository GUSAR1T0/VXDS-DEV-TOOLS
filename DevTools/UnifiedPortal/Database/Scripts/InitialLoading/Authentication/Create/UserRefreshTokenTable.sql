CREATE TABLE [authentication].[UserRefreshToken]
(
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [UserId]       INT                 NOT NULL,
    [RefreshToken] NVARCHAR(64)        NOT NULL,
    CONSTRAINT [PK_UserRefreshToken_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserRefreshToken_UserRoleId] FOREIGN KEY ([UserId]) REFERENCES [authentication].[User] ([Id]) ON DELETE CASCADE
);