using FluentMigrator.Runner.VersionTableInfo;
using VXDesign.Store.DevTools.Common.Core.Migrations;

namespace VXDesign.Store.DevTools.UnifiedPortal.Database
{
    [VersionTableMetaData]
    public class MigrationSettings : DatabaseMigrationSettings
    {
        protected override string ProjectName => "UnifiedPortal";
    }
}