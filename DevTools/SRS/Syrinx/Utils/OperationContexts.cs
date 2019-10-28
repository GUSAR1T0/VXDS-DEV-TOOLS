using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.SRS.Syrinx.Controllers;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Utils
{
    internal static class OperationContexts
    {
        private const string Authorization = "Authentication";
        private const string Camunda = "Camunda";

        #region Account

        internal static OperationContext.OperationContextBuilder SignIn(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(Authorization, nameof(AccountController.SignIn));
        }

        internal static OperationContext.OperationContextBuilder SignUp(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(Authorization, nameof(AccountController.SignUp));
        }

        internal static OperationContext.OperationContextBuilder RefreshToken(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(Authorization, nameof(AccountController.RefreshToken));
        }

        internal static OperationContext.OperationContextBuilder Logout(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(Authorization, nameof(AccountController.Logout));
        }

        internal static OperationContext.OperationContextBuilder GetUserData(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(Authorization, nameof(AccountController.GetUserData));
        }

        public static OperationContext.OperationContextBuilder VerifyAuthentication(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(Authorization, nameof(AccountController.VerifyAuthentication));
        }

        #endregion

        #region Camunda

        internal static OperationContext.OperationContextBuilder SendRequest(OperationContext.OperationContextBuilder builder)
        {
            return builder.SetName(Camunda, nameof(CamundaController.SendRequest));
        }

        #endregion
    }
}