CREATE TYPE [authentication].[UserRolePermissionItem] AS TABLE
(
    [PermissionGroupId] INT    NOT NULL,
    [Permissions]       BIGINT NOT NULL
);