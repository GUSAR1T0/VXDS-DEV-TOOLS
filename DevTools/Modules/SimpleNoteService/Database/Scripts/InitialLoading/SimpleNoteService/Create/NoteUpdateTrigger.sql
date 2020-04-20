CREATE TRIGGER [simple-note-service].[NoteUpdateTrigger]
ON [simple-note-service].[Note]
FOR UPDATE
AS
BEGIN

    UPDATE n
    SET n.[EditTime] = SYSDATETIME()
    FROM [simple-note-service].[Note] n
    INNER JOIN INSERTED i ON i.[Id] = n.[Id]
    INNER JOIN DELETED d ON d.[Id] = n.[Id]
    WHERE
        i.[Title] <> d.[Title] OR
        i.[Text] <> d.[Text];

END