CREATE TABLE [portal].[Module]
(
    [Id]       INT IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR(255)       NOT NULL,
    [Alias]    NVARCHAR(255)       NOT NULL,
    [UserId]   INT                 NOT NULL,
    [HostId]   INT                 NOT NULL,
    [IsActive] BIT DEFAULT (0)     NOT NULL,
    CONSTRAINT [PK_Module_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_Module_Alias] UNIQUE ([Alias] ASC),
    CONSTRAINT [FK_Module_UserId] FOREIGN KEY ([UserId]) REFERENCES [authentication].[User] ([Id]),
    CONSTRAINT [FK_Module_HostId] FOREIGN KEY ([HostId]) REFERENCES [portal].[Host] ([Id]) ON DELETE CASCADE
);