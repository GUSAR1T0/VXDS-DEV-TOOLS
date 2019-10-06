using System.IdentityModel.Tokens.Jwt;
using VXDesign.Store.DevTools.Common.DataStorage.Entities;
using VXDesign.Store.DevTools.Common.Entities.Authorization;
using VXDesign.Store.DevTools.SRS.Syrinx.Models.Authorization;

namespace VXDesign.Store.DevTools.SRS.Syrinx.Extensions
{
    internal static class AuthorizationModelExtensions
    {
        internal static UserModel ToModel(this UserAuthorizationEntity entity) => new UserModel
        {
            Email = entity.Email,
            FirstName = entity.FirstName,
            LastName = entity.LastName
        };

        internal static UserRegistrationEntity ToEntity(this SignUpModel model) => new UserRegistrationEntity
        {
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Password = model.Password
        };

        internal static JwtTokenModel GetJwtTokenModel(this RawJwtToken token) => new JwtTokenModel
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token.AccessToken),
            RefreshToken = token.RefreshToken
        };
    }
}