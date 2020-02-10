using FluentMigrator;
using VXDesign.Store.DevTools.Core.Storage.LogStores;

namespace VXDesign.Store.DevTools.Database.Migrations
{
    [Migration(1)]
    public class InitialLoading : Migration
    {
        private readonly ILoggerStore loggerStore;

        public InitialLoading(ILoggerStore loggerStore)
        {
            this.loggerStore = loggerStore;
        }

        public override void Up()
        {
            var authorizationSchema = Schema.Schema(Database.Schema.Authentication);
            if (!authorizationSchema.Exists())
            {
                Execute.EmbeddedScript("Authentication_CreateAuthenticationSchema.sql");
            }

            if (!authorizationSchema.Table(Table.UserRole).Exists())
            {
                Execute.EmbeddedScript("Authentication_CreateUserRoleTable.sql");
                Execute.EmbeddedScript("Authentication_AddUserRoles.sql");
            }

            if (!authorizationSchema.Table(Table.User).Exists())
            {
                Execute.EmbeddedScript("Authentication_CreateUserTable.sql");
                Execute.EmbeddedScript("Authentication_AddUsers.sql");
            }

            var baseSchema = Schema.Schema(Database.Schema.Base);
            if (!baseSchema.Exists())
            {
                Execute.EmbeddedScript("Base_CreateBaseSchema.sql");
            }

            if (!authorizationSchema.Table(Table.Operation).Exists())
            {
                Execute.EmbeddedScript("Base_CreateOperationTable.sql");
                Execute.EmbeddedScript("Base_AddInitialLoadingRecord.sql");
            }

            loggerStore.Info<InitialLoading>(0, "Database is initialized").Wait();
        }

        public override void Down()
        {
            var baseSchema = Schema.Schema(Database.Schema.Base);
            if (baseSchema.Exists())
            {
                if (baseSchema.Table(Table.Operation).Exists())
                {
                    Execute.EmbeddedScript("Base_DropOperationTable.sql");
                }

                Execute.EmbeddedScript("Base_DropBaseSchema.sql");
            }

            var authorizationSchema = Schema.Schema(Database.Schema.Authentication);
            if (authorizationSchema.Exists())
            {
                if (authorizationSchema.Table(Table.User).Exists())
                {
                    Execute.EmbeddedScript("Authentication_DropUserTable.sql");
                }

                if (authorizationSchema.Table(Table.UserRole).Exists())
                {
                    Execute.EmbeddedScript("Authentication_DropUserRoleTable.sql");
                }

                Execute.EmbeddedScript("Authentication_DropAuthenticationSchema.sql");
            }

            loggerStore.DropAllLogCollections().Wait();
        }
    }
}