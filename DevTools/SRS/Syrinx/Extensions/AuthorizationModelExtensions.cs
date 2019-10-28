using System.IdentityModel.Tokens.Jwt;
using VXDesign.Store.DevTools.Common.Entities.Storage;
using VXDesign.Store.DevTools.SRS.Authentication;
using VXDesign.Store.DevTools.SRS.Syrinx.Models.Authorization;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Extensions
{
    internal static class AuthorizationModelExtensions
    {
        internal static UserAuthorizationModel ToModel(this UserAuthorizationEntity entity) => new UserAuthorizationModel
        {
            Id = entity.Id,
            Email = entity.Email,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Color = entity.Color,
            UserPermissions = entity.UserPermissions
        };

        internal static UserRegistrationEntity ToEntity(this SignUpModel model) => new UserRegistrationEntity
        {
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Password = model.Password,
            Color = model.Color
        };

        internal static JwtTokenModel GetJwtTokenModel(this RawJwtToken token) => new JwtTokenModel
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token.AccessToken),
            RefreshToken = token.RefreshToken
        };
    }
}