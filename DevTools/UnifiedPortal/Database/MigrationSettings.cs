using FluentMigrator.Runner.VersionTableInfo;
using VXDesign.Store.DevTools.Common.Migrations.Database;

namespace VXDesign.Store.DevTools.UnifiedPortal.Database
{
    [VersionTableMetaData]
    public class MigrationSettings : DatabaseMigrationSettings
    {
        public override string TableName => "UnifiedPortal";
    }
}