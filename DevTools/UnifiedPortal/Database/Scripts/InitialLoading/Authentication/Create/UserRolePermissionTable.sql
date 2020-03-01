CREATE TABLE [authentication].[UserRolePermission]
(
    [UserRoleId]        INT     NOT NULL,
    [PermissionGroupId] TINYINT NOT NULL,
    [Permissions]       BIGINT  NOT NULL,
    CONSTRAINT [PK_UserRolePermission_UserRoleId_PermissionGroupId] PRIMARY KEY CLUSTERED ([UserRoleId] ASC, [PermissionGroupId] ASC),
    CONSTRAINT [FK_UserRolePermission_UserRoleId] FOREIGN KEY ([UserRoleId]) REFERENCES [authentication].[UserRole] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRolePermission_PermissionGroupId] FOREIGN KEY ([PermissionGroupId]) REFERENCES [enum].[PermissionGroup] ([Id]) ON DELETE CASCADE
);