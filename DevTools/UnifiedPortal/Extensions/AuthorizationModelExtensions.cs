using VXDesign.Store.DevTools.Common.Containers.DataStorage;
using VXDesign.Store.DevTools.UnifiedPortal.Models.Authorization;

namespace VXDesign.Store.DevTools.UnifiedPortal.Extensions
{
    internal static class AuthorizationModelExtensions
    {
        internal static UserAuthorizationModel ToModel(this UserAuthorizationEntity entity) => new UserAuthorizationModel
        {
            Email = entity.Email,
            FirstName = entity.FirstName,
            LastName = entity.LastName
        };

        internal static UserRegistrationEntity ToEntity(this RegistrationModel model) => new UserRegistrationEntity
        {
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Password = model.Password
        };
    }
}