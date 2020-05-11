using FluentMigrator.Runner.VersionTableInfo;
using VXDesign.Store.DevTools.Common.Migrations.Database;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Database
{
    [VersionTableMetaData]
    public class MigrationSettings : DatabaseMigrationSettings
    {
        public override string TableName => "SimpleNoteService";
    }
}