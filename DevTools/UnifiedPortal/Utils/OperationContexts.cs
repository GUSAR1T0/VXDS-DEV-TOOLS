using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.UnifiedPortal.Controllers;

namespace VXDesign.Store.DevTools.UnifiedPortal.Utils
{
    internal static class OperationContexts
    {
        private const string Lookup = "Lookup";
        private const string User = "User";
        private const string UserRole = "UserRole";

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

        #endregion
    }
}