using FluentMigrator.Runner.VersionTableInfo;
using VXDesign.Store.DevTools.Common.Core.Migrations;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Database
{
    [VersionTableMetaData]
    public class MigrationSettings : DatabaseMigrationSettings
    {
        protected override string ProjectName => "SimpleNoteService";
    }
}