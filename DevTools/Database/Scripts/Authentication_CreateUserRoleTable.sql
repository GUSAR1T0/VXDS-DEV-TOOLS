CREATE TABLE [authentication].[UserRole] (
    [Id]              INT           IDENTITY (1, 1),
    [Name]            NVARCHAR (50) NOT NULL,
    [UserPermissions] INT           NOT NULL,
    CONSTRAINT [PK_UserRole_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_UserRole_Name] UNIQUE ([Name])
);