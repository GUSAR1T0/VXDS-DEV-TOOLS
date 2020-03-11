using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Core.Entities.User;

namespace VXDesign.Store.DevTools.Common.Clients.Syrinx.Base
{
    public abstract class SyrinxVerifiedAuthenticationAttribute : TypeFilterAttribute
    {
        protected SyrinxVerifiedAuthenticationAttribute(IEnumerable<UserRolePermissionEntity> expectedUserPermissions) : base(typeof(SyrinxVerifiedAuthenticationService))
        {
            Arguments = new object[]
            {
                new Permissions
                {
                    ExpectedUserPermissions = expectedUserPermissions
                }
            };
        }
    }
}