INSERT INTO [enum].[IncidentStatus] ([Name])
SELECT N'Opened'
UNION ALL
SELECT N'In Progress'
UNION ALL
SELECT N'Resolved'
UNION ALL
SELECT N'Cancelled';