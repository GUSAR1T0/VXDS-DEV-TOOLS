using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Core.Entities.Operations;
using VXDesign.Store.DevTools.Core.Entities.Storage.User;
using VXDesign.Store.DevTools.Core.Services.Syrinx;

namespace VXDesign.Store.DevTools.Core.Attributes
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