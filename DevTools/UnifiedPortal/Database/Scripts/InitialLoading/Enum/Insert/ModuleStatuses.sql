INSERT INTO [enum].[ModuleStatus] ([Name])
SELECT N'Created'
UNION ALL
SELECT N'Installing'
UNION ALL
SELECT N'Installed'
UNION ALL
SELECT N'Failed To Install'
UNION ALL
SELECT N'Updated To Upgrade'
UNION ALL
SELECT N'Upgrading'
UNION ALL
SELECT N'Upgraded'
UNION ALL
SELECT N'Failed To Upgrade'
UNION ALL
SELECT N'Updated To Downgrade'
UNION ALL
SELECT N'Downgrading'
UNION ALL
SELECT N'Downgraded'
UNION ALL
SELECT N'Failed To Downgrade'
UNION ALL
SELECT N'Updated To Uninstall'
UNION ALL
SELECT N'Uninstalling'
UNION ALL
SELECT N'Uninstalled'
UNION ALL
SELECT N'Failed To Uninstall'
UNION ALL
SELECT N'Updated To Run'
UNION ALL
SELECT N'Running'
UNION ALL
SELECT N'Run'
UNION ALL
SELECT N'Failed To Run'
UNION ALL
SELECT N'Updated To Stop'
UNION ALL
SELECT N'Stopping'
UNION ALL
SELECT N'Stopped'
UNION ALL
SELECT N'Failed To Stop';