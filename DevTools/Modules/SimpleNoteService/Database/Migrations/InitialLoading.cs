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
            .SetName(GetType().FullName, "InitialLoading")
            .SetUserId(null, true)
            .Create();

        private readonly IOperationService operationService;

        public InitialLoading(IOperationService operationService)
        {
            this.operationService = operationService;
        }

        #region Upgrade

        public override void Up()
        {
            operationService.Make(Context, async operation =>
            {
                UpgradeEnumSchema(operation);
                UpgradeModuleSchema(operation);
            }).Wait();
        }

        private void UpgradeEnumSchema(IOperation operation)
        {
            var schema = Schema.Schema(Database.Schema.Enum);
            if (!schema.Exists())
            {
                throw CommonExceptions.DatabaseSchemaWasNotFound(operation, Database.Schema.Enum);
            }

            if (schema.Table(Table.PermissionGroup).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Enum.Insert.PermissionGroups.sql");
            }
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
            operationService.Make(Context, async operation =>
            {
                DowngradeModuleSchema(operation);
                DowngradeEnumSchema(operation);
            }).Wait();
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

        private void DowngradeEnumSchema(IOperation operation)
        {
            var schema = Schema.Schema(Database.Schema.Enum);
            if (!schema.Exists())
            {
                throw CommonExceptions.DatabaseSchemaWasNotFound(operation, Database.Schema.Enum);
            }

            if (schema.Table(Table.PermissionGroup).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Enum.Delete.PermissionGroups.sql");
            }
        }

        #endregion
    }
}