using FluentMigrator.Runner;
using VXDesign.Store.DevTools.Common.Migrations.Common;

namespace VXDesign.Store.DevTools.Common.Migrations.Database
{
    public interface IDatabaseMigrationService : IMigrationService
    {
    }

    public class DatabaseMigrationService : IDatabaseMigrationService
    {
        private readonly IMigrationRunner runner;

        public DatabaseMigrationService(IMigrationRunner runner)
        {
            this.runner = runner;
        }

        public void Upgrade() => runner.UpgradeDatabase();

        public void DowngradeToPrevious()
        {
            if (runner.HasMigrationsToApplyRollback())
            {
                runner.Rollback(1);
            }
        }

        public void Downgrade() => runner.DowngradeDatabase();
    }
}