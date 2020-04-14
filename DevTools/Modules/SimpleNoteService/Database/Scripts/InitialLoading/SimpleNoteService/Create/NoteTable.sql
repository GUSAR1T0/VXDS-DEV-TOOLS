CREATE TABLE [simple-note-service].[Note]
(
    [Id]       INT IDENTITY (1, 1)                NOT NULL,
    [FolderId] INT                                NOT NULL,
    [UserId]   INT                                NOT NULL,
    [EditTime] DATETIME2(3) DEFAULT SYSDATETIME() NOT NULL,
    [Title]    NVARCHAR(255)                      NOT NULL,
    [Text]     NVARCHAR(MAX)                      NULL,
    CONSTRAINT [PK_Note_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Note_FolderId] FOREIGN KEY ([FolderId]) REFERENCES [simple-note-service].[Folder] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Note_UserId] FOREIGN KEY ([UserId]) REFERENCES [authentication].[User] ([Id]) ON DELETE CASCADE
);