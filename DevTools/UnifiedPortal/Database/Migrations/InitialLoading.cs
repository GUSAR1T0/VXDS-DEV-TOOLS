using FluentMigrator;
using VXDesign.Store.DevTools.Common.Storage.LogStorage.Stores;

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

        #region Upgrade

        public override void Up()
        {
            UpgradeEnumSchema();
            UpgradeAuthenticationSchema();
            UpgradeBaseSchema();
            UpgradePortalSchema();

            loggerStore.Info<InitialLoading>(0, "Database is initialized").Wait();
        }

        private void UpgradeEnumSchema()
        {
            var schema = Schema.Schema(Database.Schema.Enum);
            if (!schema.Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Enum.Create.Schema.sql");
            }

            if (!schema.Table(Table.PermissionGroup).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Enum.Create.PermissionGroupTable.sql");
                Execute.EmbeddedScript("InitialLoading.Enum.Insert.PermissionGroups.sql");
            }

            if (!schema.Table(Table.Permission).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Enum.Create.PermissionTable.sql");
                Execute.EmbeddedScript("InitialLoading.Enum.Insert.Permissions.sql");
            }
        }

        private void UpgradeAuthenticationSchema()
        {
            var schema = Schema.Schema(Database.Schema.Authentication);
            if (!schema.Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Authentication.Create.Schema.sql");
                Execute.EmbeddedScript("InitialLoading.Authentication.Create.UserRolePermissionTableType.sql");
            }

            if (!schema.Table(Table.UserRole).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Authentication.Create.UserRoleTable.sql");
                Execute.EmbeddedScript("InitialLoading.Authentication.Insert.UserRoles.sql");
            }

            if (!schema.Table(Table.UserRolePermission).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Authentication.Create.UserRolePermissionTable.sql");
                Execute.EmbeddedScript("InitialLoading.Authentication.Insert.UserRolePermissions.sql");
            }

            if (!schema.Table(Table.User).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Authentication.Create.UserTable.sql");
                Execute.EmbeddedScript("InitialLoading.Authentication.Insert.Users.sql");
            }
        }

        private void UpgradeBaseSchema()
        {
            var schema = Schema.Schema(Database.Schema.Base);
            if (!schema.Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Base.Create.Schema.sql");
                Execute.EmbeddedScript("InitialLoading.Base.Create.ListTableTypes.sql");
            }

            if (!schema.Table(Table.Operation).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Base.Create.OperationTable.sql");
                Execute.EmbeddedScript("InitialLoading.Base.Insert.InitialLoadingRecord.sql");
            }
        }

        private void UpgradePortalSchema()
        {
            var schema = Schema.Schema(Database.Schema.Portal);
            if (!schema.Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Portal.Create.Schema.sql");
            }

            if (!schema.Table(Table.PortalSettings).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Portal.Create.SettingsTable.sql");
            }

            if (!schema.Table(Table.Project).Exists())
            {
                Execute.EmbeddedScript("InitialLoading.Portal.Create.ProjectTable.sql");
            }
        }

        #endregion

        #region Downgrade

        public override void Down()
        {
            DowngradePortalSchema();
            DowngradeBaseSchema();
            DowngradeAuthenticationSchema();
            DowngradeEnumSchema();

            loggerStore.DropAllLogCollections().Wait();
        }

        private void DowngradePortalSchema()
        {
            var schema = Schema.Schema(Database.Schema.Portal);
            if (schema.Exists())
            {
                if (schema.Table(Table.PortalSettings).Exists())
                {
                    Execute.EmbeddedScript("InitialLoading.Portal.Drop.SettingsTable.sql");
                }

                if (schema.Table(Table.Project).Exists())
                {
                    Execute.EmbeddedScript("InitialLoading.Portal.Drop.ProjectTable.sql");
                }

                Execute.EmbeddedScript("InitialLoading.Portal.Drop.Schema.sql");
            }
        }

        private void DowngradeBaseSchema()
        {
            var schema = Schema.Schema(Database.Schema.Base);
            if (schema.Exists())
            {
                if (schema.Table(Table.Operation).Exists())
                {
                    Execute.EmbeddedScript("InitialLoading.Base.Drop.OperationTable.sql");
                }

                Execute.EmbeddedScript("InitialLoading.Base.Drop.ListTableTypes.sql");
                Execute.EmbeddedScript("InitialLoading.Base.Drop.Schema.sql");
            }
        }

        private void DowngradeAuthenticationSchema()
        {
            var schema = Schema.Schema(Database.Schema.Authentication);
            if (schema.Exists())
            {
                if (schema.Table(Table.User).Exists())
                {
                    Execute.EmbeddedScript("InitialLoading.Authentication.Drop.UserTable.sql");
                }

                if (schema.Table(Table.UserRolePermission).Exists())
                {
                    Execute.EmbeddedScript("InitialLoading.Authentication.Drop.UserRolePermissionTable.sql");
                }

                if (schema.Table(Table.UserRole).Exists())
                {
                    Execute.EmbeddedScript("InitialLoading.Authentication.Drop.UserRoleTable.sql");
                }

                Execute.EmbeddedScript("InitialLoading.Authentication.Drop.UserRolePermissionTableType.sql");
                Execute.EmbeddedScript("InitialLoading.Authentication.Drop.Schema.sql");
            }
        }

        private void DowngradeEnumSchema()
        {
            var schema = Schema.Schema(Database.Schema.Enum);
            if (schema.Exists())
            {
                if (schema.Table(Table.Permission).Exists())
                {
                    Execute.EmbeddedScript("InitialLoading.Enum.Drop.PermissionTable.sql");
                }

                if (schema.Table(Table.PermissionGroup).Exists())
                {
                    Execute.EmbeddedScript("InitialLoading.Enum.Drop.PermissionGroupTable.sql");
                }

                Execute.EmbeddedScript("InitialLoading.Enum.Drop.Schema.sql");
            }
        }

        #endregion
    }
}