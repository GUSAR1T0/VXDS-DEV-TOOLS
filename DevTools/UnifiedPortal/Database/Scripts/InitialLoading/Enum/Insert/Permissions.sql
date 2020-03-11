INSERT INTO [enum].[Permission] ([Id], [PermissionGroupId], [Name])
SELECT 1, 1, N'Access to Admin Panel'
UNION ALL
SELECT 2, 1, N'Manage User Profiles'
UNION ALL
SELECT 4, 1, N'Manage User Roles'
UNION ALL
SELECT 8, 1, N'Manage Settings'
UNION ALL
SELECT 16, 1, N'Manage Projects';