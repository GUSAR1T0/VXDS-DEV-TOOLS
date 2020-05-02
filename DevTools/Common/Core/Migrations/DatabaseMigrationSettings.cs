using FluentMigrator.Runner.VersionTableInfo;

namespace VXDesign.Store.DevTools.Common.Core.Migrations
{
    public abstract class DatabaseMigrationSettings : DefaultVersionTableMetaData
    {
        public override string SchemaName => "migration";
        protected abstract string ProjectName { get; }
        public sealed override string TableName => $"{ProjectName}Database";
    }
}