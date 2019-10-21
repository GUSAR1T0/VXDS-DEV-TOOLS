using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Entities.Operations;
using VXDesign.Store.DevTools.Common.Enums.Operations;
using VXDesign.Store.DevTools.Common.Services.Syrinx;

namespace VXDesign.Store.DevTools.Common.Attributes
{
    public class SyrinxVerifiedAuthenticationAttribute : TypeFilterAttribute
    {
        public SyrinxVerifiedAuthenticationAttribute(UserPermission userPermissions = 0, UserRolePermission userRolePermissions = 0) : base(typeof(SyrinxVerifiedAuthenticationService))
        {
            Arguments = new object[]
            {
                new Permissions
                {
                    UserPermissions = userPermissions,
                    UserRolePermissions = userRolePermissions
                }
            };
        }
    }
}