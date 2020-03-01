CREATE TABLE [enum].[Permission]
(
    [Id]                BIGINT        NOT NULL,
    [PermissionGroupId] TINYINT       NOT NULL,
    [Name]              NVARCHAR(255) NOT NULL,
    CONSTRAINT [PK_Permission_Id_PermissionGroupId] PRIMARY KEY CLUSTERED ([Id] ASC, [PermissionGroupId] ASC),
    CONSTRAINT [FK_Permission_PermissionGroupId] FOREIGN KEY ([PermissionGroupId]) REFERENCES [enum].[PermissionGroup] ([Id]) ON DELETE CASCADE
);