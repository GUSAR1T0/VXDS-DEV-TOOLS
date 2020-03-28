INSERT INTO [enum].[NotificationLevel] ([Name])
SELECT N'Information'
UNION ALL
SELECT N'Warning'
UNION ALL
SELECT N'Error';