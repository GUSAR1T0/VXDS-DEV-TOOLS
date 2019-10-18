using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.UnifiedPortal.Controllers;

namespace VXDesign.Store.DevTools.UnifiedPortal.Utils
{
    internal static class OperationContexts
    {
        private const string User = "User";
        private const string UserRole = "UserRole";

        #region User

        internal static OperationContext GetUserProfile() => OperationContext.Create(User, nameof(UserController.GetUserProfile));
        internal static OperationContext UpdateUserProfile() => OperationContext.Create(User, nameof(UserController.UpdateUserProfile));

        #endregion

        #region UserRole

        internal static OperationContext GetUserRoles() => OperationContext.Create(UserRole, nameof(UserRoleController.GetUserRoles));
        internal static OperationContext AddUserRole() => OperationContext.Create(UserRole, nameof(UserRoleController.AddUserRole));
        internal static OperationContext UpdateUserRole() => OperationContext.Create(UserRole, nameof(UserRoleController.UpdateUserRole));
        internal static OperationContext DeleteUserRole() => OperationContext.Create(UserRole, nameof(UserRoleController.DeleteUserRole));

        #endregion
    }
}