INSERT INTO [authentication].[UserRole] ([Name], [PortalPermissions])
SELECT N'Administrator', 7
UNION ALL
SELECT N'Teammate', 0;