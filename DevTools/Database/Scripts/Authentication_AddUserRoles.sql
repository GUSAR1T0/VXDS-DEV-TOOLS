INSERT INTO [authentication].[UserRole] ([Name], [UserPermissions])
SELECT N'Administrator', 7
UNION ALL
SELECT N'Teammate', 0;