using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.UnifiedPortal.Controllers;

namespace VXDesign.Store.DevTools.UnifiedPortal.Utils
{
    internal static class OperationContexts
    {
        private const string User = "User";
        private const string UserRole = "UserRole";

        #region User

        internal static OperationContext.OperationContextBuilder GetUserProfile(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(User, nameof(UserController.GetUserProfile));
        }

        internal static OperationContext.OperationContextBuilder UpdateUserProfile(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(User, nameof(UserController.UpdateUserProfile));
        }

        #endregion

        #region UserRole

        internal static OperationContext.OperationContextBuilder GetUserRoles(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(UserRole, nameof(UserRoleController.GetUserRoles));
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