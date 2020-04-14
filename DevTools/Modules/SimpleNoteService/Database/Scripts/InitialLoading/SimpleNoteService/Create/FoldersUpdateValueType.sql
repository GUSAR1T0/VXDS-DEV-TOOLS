CREATE TYPE [simple-note-service].[FoldersUpdateValue] AS TABLE
(
    [Id]    INT           NOT NULL,
    [Nodes] NVARCHAR(MAX) NULL
);