using FluentMigrator;
using VXDesign.Store.DevTools.Common.Core.Exceptions;
using VXDesign.Store.DevTools.Common.Core.Operations;
using VXDesign.Store.DevTools.Common.Storage.LogStorage.Stores;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Database.Migrations
{
    [Migration(1)]
    public class InitialLoading : Migration
    {
        private OperationContext Context => OperationContext.Builder()
            .SetName(GetType().FullName, "SimpleNoteService", "Migration", "InitialLoading")
            .Create();

        private readonly IOperationService operationService;

        public InitialLoading(IOperationService operationService)
        {
            this.operationService = operationService;
        }

        #region Upgrade

        public override void Up()
        {
            operationService.Make(Context, async operation => UpgradeModuleSchema(operation)).Wait();
        }

        private void UpgradeModuleSchema(IOperation operation)
        {
            var schema = Schema.Schema(Database.Schema.Module);
            if (!schema.Exists())
            {
                throw CommonExceptions.DatabaseSchemaWasNotFound(operation, Database.Schema.Module);
            }

            if (!schema.Table(Table.SimpleNoteService).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Module.Create.SimpleNoteServiceTable.sql");
            }
        }

        #endregion

        #region Downgrade

        public override void Down()
        {
            operationService.Make(Context, async operation => DowngradeModuleSchema(operation)).Wait();
        }

        private void DowngradeModuleSchema(IOperation operation)
        {
            var schema = Schema.Schema(Database.Schema.Module);
            if (!schema.Exists())
            {
                throw CommonExceptions.DatabaseSchemaWasNotFound(operation, Database.Schema.Module);
            }

            if (schema.Table(Table.SimpleNoteService).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Module.Drop.SimpleNoteServiceTable.sql");
            }
        }

        #endregion
    }
}