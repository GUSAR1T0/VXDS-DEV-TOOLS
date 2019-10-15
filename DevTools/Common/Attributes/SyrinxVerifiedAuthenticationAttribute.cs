using Microsoft.AspNetCore.Mvc;
using VXDesign.Store.DevTools.Common.Services.Syrinx;

namespace VXDesign.Store.DevTools.Common.Attributes
{
    public class SyrinxVerifiedAuthenticationAttribute : TypeFilterAttribute
    {
        public SyrinxVerifiedAuthenticationAttribute() : base(typeof(SyrinxVerifiedAuthenticationService))
        {
        }
    }
}