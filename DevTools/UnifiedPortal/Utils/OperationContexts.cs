using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.UnifiedPortal.Controllers;

namespace VXDesign.Store.DevTools.UnifiedPortal.Utils
{
    internal static class OperationContexts
    {
        private const string Lookup = "Lookup";
        private const string User = "User";
        private const string UserRole = "UserRole";
        private const string AdminPanel = "AdminPanel";

        #region Lookup

        internal static OperationContext.OperationContextBuilder GetAllValues(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(Lookup, nameof(LookupController.GetAllValues));
        }

        #endregion

        #region User

        internal static OperationContext.OperationContextBuilder GetUsers(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(User, nameof(UserController.GetUsers));
        }

        internal static OperationContext.OperationContextBuilder GetUserProfile(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(User, nameof(UserController.GetUserProfile));
        }

        internal static OperationContext.OperationContextBuilder UpdateUserProfileGeneralInfo(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(User, nameof(UserController.UpdateUserProfileGeneralInfo));
        }

        internal static OperationContext.OperationContextBuilder UpdateUserProfileAccountSpecificInfo(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(User, nameof(UserController.UpdateUserProfileAccountSpecificInfo));
        }

        internal static OperationContext.OperationContextBuilder DeactivateUser(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(User, nameof(UserController.DeactivateUser));
        }

        internal static OperationContext.OperationContextBuilder ActivateUser(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(User, nameof(UserController.ActivateUser));
        }

        #endregion

        #region UserRole

        internal static OperationContext.OperationContextBuilder GetUserRolesFullInfo(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(UserRole, nameof(UserRoleController.GetUserRolesFullInfo));
        }

        internal static OperationContext.OperationContextBuilder GetUserRolesShortInfo(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(UserRole, nameof(UserRoleController.GetUserRolesShortInfo));
        }

        internal static OperationContext.OperationContextBuilder GetUserRole(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(UserRole, nameof(UserRoleController.GetUserRole));
        }

        internal static OperationContext.OperationContextBuilder AddUserRole(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(UserRole, nameof(UserRoleController.AddUserRole));
        }

        internal static OperationContext.OperationContextBuilder UpdateUserRole(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(UserRole, nameof(UserRoleController.UpdateUserRole));
        }

        internal static OperationContext.OperationContextBuilder DeleteUserRole(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(UserRole, nameof(UserRoleController.DeleteUserRole));
        }

        internal static OperationContext.OperationContextBuilder GetAffectedUsersCount(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(UserRole, nameof(UserRoleController.GetAffectedUsersCount));
        }

        #endregion

        #region AdminPanel

        internal static OperationContext.OperationContextBuilder GetAdminPanelData(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(AdminPanel, nameof(DashboardController.GetAdminPanelData));
        }

        #endregion
    }
}