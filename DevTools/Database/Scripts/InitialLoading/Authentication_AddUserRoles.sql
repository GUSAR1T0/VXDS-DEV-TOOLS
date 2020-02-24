INSERT INTO [authentication].[UserRole] ([Name], [PortalPermissions])
SELECT N'Administrator', 31
UNION ALL
SELECT N'Manager', 1
UNION ALL
SELECT N'Teammate', 0;