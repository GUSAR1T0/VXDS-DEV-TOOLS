CREATE TABLE [simple-note-service].[Folder]
(
    [Id]    INT IDENTITY (1, 1) NOT NULL,
    [Name]  NVARCHAR(100)       NOT NULL,
    [Nodes] NVARCHAR(MAX)       NULL,
    CONSTRAINT [PK_Folder_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);