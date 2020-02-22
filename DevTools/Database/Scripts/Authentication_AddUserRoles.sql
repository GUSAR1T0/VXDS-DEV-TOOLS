INSERT INTO [authentication].[UserRole] ([Name], [PortalPermissions])
SELECT N'Administrator', 15
UNION ALL
SELECT N'Manager', 1
UNION ALL
SELECT N'Teammate', 0;