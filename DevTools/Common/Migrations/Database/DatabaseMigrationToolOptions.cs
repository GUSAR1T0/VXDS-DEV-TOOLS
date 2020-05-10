using CommandLine;
using VXDesign.Store.DevTools.Common.Migrations.Common;

namespace VXDesign.Store.DevTools.Common.Migrations.Database
{
    public class DatabaseMigrationToolOptions
    {
        [Value(0, Required = true)]
        public MigrationAction Action { get; set; }
    }
}