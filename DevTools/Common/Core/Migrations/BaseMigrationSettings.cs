using FluentMigrator.Runner.VersionTableInfo;

namespace VXDesign.Store.DevTools.Common.Core.Migrations
{
    public abstract class BaseMigrationSettings : DefaultVersionTableMetaData
    {
        public override string SchemaName => "migration";
    }
}