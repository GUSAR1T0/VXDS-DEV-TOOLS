SET IDENTITY_INSERT [enum].[FileExtension] ON;

INSERT INTO [enum].[FileExtension] ([Id], [Name])
SELECT 0, N'Undefined';

SET IDENTITY_INSERT [enum].[FileExtension] OFF;

INSERT INTO [enum].[FileExtension] ([Name])
SELECT N'YAML/YML'
UNION ALL
SELECT N'JSON';