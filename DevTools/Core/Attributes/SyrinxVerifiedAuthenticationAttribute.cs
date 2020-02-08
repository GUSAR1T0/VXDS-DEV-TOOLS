using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Core.Entities.Operations;
using VXDesign.Store.DevTools.Core.Enums.Operations;
using VXDesign.Store.DevTools.Core.Services.Syrinx;

namespace VXDesign.Store.DevTools.Core.Attributes
{
    public class SyrinxVerifiedAuthenticationAttribute : TypeFilterAttribute
    {
        public SyrinxVerifiedAuthenticationAttribute(PortalPermission portalPermissions = 0) : base(typeof(SyrinxVerifiedAuthenticationService))
        {
            Arguments = new object[]
            {
                new Permissions
                {
                    PortalPermissions = portalPermissions
                }
            };
        }
    }
}