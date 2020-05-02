using FluentMigrator.Runner;

namespace VXDesign.Store.DevTools.Common.Core.Extensions
{
    public static class DatabaseMigrationRunnerExtensions
    {
        public static void UpgradeDatabase(this IMigrationRunner runner, long? version = null)
        {
            if (version == null) runner.MigrateUp();
            else runner.MigrateUp(version.Value);
        }

        public static void DowngradeDatabase(this IMigrationRunner runner, long? version = null) => runner.MigrateDown(version ?? 0);
    }
}