using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.SRS.Syrinx.Controllers;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Utils
{
    internal static class OperationContexts
    {
        private const string Server = "Syrinx";

        private const string Authorization = "Authorization";
        private const string Camunda = "Camunda";

        #region Account

        internal static OperationContext SignIn() => OperationContext.Create(Server, Authorization, nameof(AccountController.SignIn));
        internal static OperationContext SignUp() => OperationContext.Create(Server, Authorization, nameof(AccountController.SignUp));
        internal static OperationContext RefreshToken() => OperationContext.Create(Server, Authorization, nameof(AccountController.RefreshToken));
        internal static OperationContext Logout() => OperationContext.Create(Server, Authorization, nameof(AccountController.Logout));
        internal static OperationContext GetUserData() => OperationContext.Create(Server, Authorization, nameof(AccountController.GetUserData));

        #endregion

        #region Camunda

        internal static OperationContext SendRequest() => OperationContext.Create(Server, Camunda, nameof(CamundaController.SendRequest));

        #endregion
    }
}