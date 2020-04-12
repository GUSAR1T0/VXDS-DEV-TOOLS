DECLARE @PermissionGroupIds TABLE ([Id] INT);

INSERT INTO [enum].[PermissionGroup] ([Name])
OUTPUT INSERTED.[Id] INTO @PermissionGroupIds
SELECT N'Simple Note Service Access & Management';

INSERT INTO [enum].[Permission] ([Id], [PermissionGroupId], [Name])
SELECT t.[Id], i.[Id], t.[Name]
FROM (
    SELECT 1 [Id], N'Access to Module' [Name]
) t
CROSS APPLY @PermissionGroupIds i;

INSERT INTO [authentication].[UserRolePermission] ([UserRoleId], [PermissionGroupId], [Permissions])
SELECT 1, i.[Id], 1
FROM @PermissionGroupIds i;