DECLARE @Now DATETIMEOFFSET = SYSDATETIMEOFFSET();

SET IDENTITY_INSERT [base].[Operation] ON;

INSERT INTO [base].[Operation] ([Id], [Scope], [ContextName], [IsSystemAction], [IsSuccessful], StartTime, StopTime)
VALUES (0, 'VXDS_DB', 'VXDesign.Store.DevTools.Database.Migrations.InitialLoading::Up', 1, 1, @Now, @Now);

SET IDENTITY_INSERT [base].[Operation] OFF;