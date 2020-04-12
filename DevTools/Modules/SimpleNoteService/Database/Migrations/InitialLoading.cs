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
                UpgradeSimpleNoteServiceSchema();
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

        private void UpgradeSimpleNoteServiceSchema()
        {
            var schema = Schema.Schema(Database.Schema.SimpleNoteService);
            if (!schema.Exists())
            {
                Execute.EmbeddedScript("InitialLoading.SimpleNoteService.Create.Schema.sql");
            }

            if (!schema.Table(Table.Note).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.SimpleNoteService.Create.NoteTable.sql");
            }

            if (!schema.Table(Table.NoteProject).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.SimpleNoteService.Create.NoteProjectTable.sql");
            }
        }

        #endregion

        #region Downgrade

        public override void Down()
        {
            operationService.Make(Context, async operation =>
            {
                DowngradeSimpleNoteServiceSchema();
                DowngradeEnumSchema(operation);
            }).Wait();
        }

        private void DowngradeSimpleNoteServiceSchema()
        {
            var schema = Schema.Schema(Database.Schema.SimpleNoteService);
            if (schema.Exists())
            {
                if (schema.Table(Table.NoteProject).Exists())
                {
                    Execute.EmbeddedScript("InitialLoading.SimpleNoteService.Drop.NoteProjectTable.sql");
                }

                if (schema.Table(Table.Note).Exists())
                {
                    Execute.EmbeddedScript("InitialLoading.SimpleNoteService.Drop.NoteTable.sql");
                }

                Execute.EmbeddedScript("InitialLoading.SimpleNoteService.Drop.Schema.sql");
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