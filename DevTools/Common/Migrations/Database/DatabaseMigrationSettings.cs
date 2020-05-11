using FluentMigrator.Runner.VersionTableInfo;

namespace VXDesign.Store.DevTools.Common.Migrations.Database
{
    public abstract class DatabaseMigrationSettings : DefaultVersionTableMetaData
    {
        public sealed override string SchemaName => "migration";
    }
}