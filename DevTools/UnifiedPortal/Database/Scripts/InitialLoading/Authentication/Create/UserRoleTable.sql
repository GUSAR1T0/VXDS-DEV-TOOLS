CREATE TABLE [authentication].[UserRole]
(
    [Id]   INT IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR(50)        NOT NULL,
    CONSTRAINT [PK_UserRole_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);