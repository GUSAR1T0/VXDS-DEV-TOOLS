using FluentMigrator;
using VXDesign.Store.DevTools.Core.Storage.LogStores;

namespace VXDesign.Store.DevTools.UnifiedPortal.Database.Migrations
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
            var enumSchema = Schema.Schema(Database.Schema.Enum);
            if (!enumSchema.Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Enum_CreateEnumSchema.sql");
            }

            if (!enumSchema.Table(Table.PermissionGroup).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Enum_CreatePermissionGroupTable.sql");
                Execute.EmbeddedScript("InitialLoading.Enum_AddPermissionGroups.sql");
            }

            if (!enumSchema.Table(Table.Permission).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Enum_CreatePermissionTable.sql");
                Execute.EmbeddedScript("InitialLoading.Enum_AddPermissions.sql");
            }

            var authorizationSchema = Schema.Schema(Database.Schema.Authentication);
            if (!authorizationSchema.Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Authentication_CreateAuthenticationSchema.sql");
                Execute.EmbeddedScript("InitialLoading.Authentication_CreateUserRolePermissionTableType.sql");
            }

            if (!authorizationSchema.Table(Table.UserRole).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Authentication_CreateUserRoleTable.sql");
                Execute.EmbeddedScript("InitialLoading.Authentication_AddUserRoles.sql");
            }

            if (!authorizationSchema.Table(Table.UserRolePermission).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Authentication_CreateUserRolePermissionTable.sql");
                Execute.EmbeddedScript("InitialLoading.Authentication_AddUserRolePermissions.sql");
            }

            if (!authorizationSchema.Table(Table.User).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Authentication_CreateUserTable.sql");
                Execute.EmbeddedScript("InitialLoading.Authentication_AddUsers.sql");
            }

            var baseSchema = Schema.Schema(Database.Schema.Base);
            if (!baseSchema.Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Base_CreateBaseSchema.sql");
                Execute.EmbeddedScript("InitialLoading.Base_CreateListTableTypes.sql");
            }

            if (!baseSchema.Table(Table.Operation).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Base_CreateOperationTable.sql");
                Execute.EmbeddedScript("InitialLoading.Base_AddInitialLoadingRecord.sql");
            }

            var portalSchema = Schema.Schema(Database.Schema.Portal);
            if (!portalSchema.Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Portal_CreatePortalSchema.sql");
            }

            if (!portalSchema.Table(Table.PortalSettings).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Portal_CreateSettingsTable.sql");
            }

            if (!portalSchema.Table(Table.Project).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Portal_CreateProjectTable.sql");
            }

            loggerStore.Info<InitialLoading>(0, "Database is initialized").Wait();
        }

        public override void Down()
        {
            var portalSchema = Schema.Schema(Database.Schema.Portal);
            if (portalSchema.Exists())
            {
                if (portalSchema.Table(Table.PortalSettings).Exists())
                {
                    Execute.EmbeddedScript("InitialLoading.Portal_DropSettingsTable.sql");
                }

                if (portalSchema.Table(Table.Project).Exists())
                {
                    Execute.EmbeddedScript("InitialLoading.Portal_DropProjectTable.sql");
                }

                Execute.EmbeddedScript("InitialLoading.Portal_DropPortalSchema.sql");
            }

            var baseSchema = Schema.Schema(Database.Schema.Base);
            if (baseSchema.Exists())
            {
                if (baseSchema.Table(Table.Operation).Exists())
                {
                    Execute.EmbeddedScript("InitialLoading.Base_DropOperationTable.sql");
                }

                Execute.EmbeddedScript("InitialLoading.Base_DropListTableTypes.sql");
                Execute.EmbeddedScript("InitialLoading.Base_DropBaseSchema.sql");
            }

            var authorizationSchema = Schema.Schema(Database.Schema.Authentication);
            if (authorizationSchema.Exists())
            {
                if (authorizationSchema.Table(Table.User).Exists())
                {
                    Execute.EmbeddedScript("InitialLoading.Authentication_DropUserTable.sql");
                }

                if (authorizationSchema.Table(Table.UserRolePermission).Exists())
                {
                    Execute.EmbeddedScript("InitialLoading.Authentication_DropUserRolePermissionTable.sql");
                }

                if (authorizationSchema.Table(Table.UserRole).Exists())
                {
                    Execute.EmbeddedScript("InitialLoading.Authentication_DropUserRoleTable.sql");
                }

                Execute.EmbeddedScript("InitialLoading.Authentication_DropUserRolePermissionTableType.sql");
                Execute.EmbeddedScript("InitialLoading.Authentication_DropAuthenticationSchema.sql");
            }

            var enumSchema = Schema.Schema(Database.Schema.Enum);
            if (enumSchema.Exists())
            {
                if (enumSchema.Table(Table.Permission).Exists())
                {
                    Execute.EmbeddedScript("InitialLoading.Enum_DropPermissionTable.sql");
                }

                if (enumSchema.Table(Table.PermissionGroup).Exists())
                {
                    Execute.EmbeddedScript("InitialLoading.Enum_DropPermissionGroupTable.sql");
                }

                Execute.EmbeddedScript("InitialLoading.Enum_DropEnumSchema.sql");
            }
 
            loggerStore.DropAllLogCollections().Wait();
        }
    }
}