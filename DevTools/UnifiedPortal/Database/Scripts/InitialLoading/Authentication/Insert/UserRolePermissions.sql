INSERT INTO [authentication].[UserRolePermission] ([UserRoleId], [PermissionGroupId], [Permissions])
SELECT 1, 1, 63
UNION ALL
SELECT 2, 1, 1
UNION ALL
SELECT 3, 1, 0;