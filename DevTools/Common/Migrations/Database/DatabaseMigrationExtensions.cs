using FluentMigrator.Runner;

namespace VXDesign.Store.DevTools.Common.Migrations.Database
{
    internal static class DatabaseMigrationExtensions
    {
        internal static void UpgradeDatabase(this IMigrationRunner runner, long? version = null)
        {
            if (version == null) runner.MigrateUp();
            else runner.MigrateUp(version.Value);
        }

        internal static void DowngradeDatabase(this IMigrationRunner runner, long? version = null) => runner.MigrateDown(version ?? 0);
    }
}