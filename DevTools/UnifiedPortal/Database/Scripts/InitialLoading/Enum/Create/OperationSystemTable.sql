CREATE TABLE [enum].[OperationSystem]
(
    [Id]   TINYINT IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR(255)           NOT NULL,
    CONSTRAINT [PK_OperationSystem_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);