CREATE TABLE [portal].[Module]
(
    [Id]     INT IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR(64)        NOT NULL,
    [UserId] INT                 NOT NULL,
    CONSTRAINT [PK_Module_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Module_Name] UNIQUE ([Name] ASC),
    CONSTRAINT [FK_Module_UserId] FOREIGN KEY ([UserId]) REFERENCES [authentication].[User] ([Id])
);