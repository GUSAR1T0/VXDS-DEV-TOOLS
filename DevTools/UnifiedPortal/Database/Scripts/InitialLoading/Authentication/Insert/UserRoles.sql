INSERT INTO [authentication].[UserRole] ([Name])
SELECT N'Administrator'
UNION ALL
SELECT N'Manager'
UNION ALL
SELECT N'Teammate';