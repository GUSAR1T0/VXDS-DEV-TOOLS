CREATE TABLE [simple-note-service].[NoteProject]
(
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [NoteId]    INT                 NOT NULL,
    [ProjectId] INT                 NOT NULL,
    CONSTRAINT [PK_NoteProject_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_NoteProject_NoteId] FOREIGN KEY ([NoteId]) REFERENCES [simple-note-service].[Note] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_NoteProject_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [portal].[Project] ([Id]) ON DELETE CASCADE
);