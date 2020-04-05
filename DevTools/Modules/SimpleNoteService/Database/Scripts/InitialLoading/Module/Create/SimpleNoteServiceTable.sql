CREATE TABLE [module].[SimpleNoteService]
(
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [UserId]       INT                 NOT NULL,
    [CreationTime] DATETIME2           NOT NULL,
    [EditTime]     DATETIME2           NOT NULL,
    [Text]         NVARCHAR(MAX)       NULL,
    CONSTRAINT [PK_SimpleNoteService_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SimpleNoteService_UserId] FOREIGN KEY ([UserId]) REFERENCES [authentication].[User] ([Id])
);