using VXDesign.Store.DevTools.Common.DataStorage.Entities;
using VXDesign.Store.DevTools.UnifiedPortal.Models.User;

namespace VXDesign.Store.DevTools.UnifiedPortal.Extensions
{
    internal static class UserModelExtensions
    {
        internal static UserProfileGetModel ToModel(this UserProfileEntity entity) => new UserProfileGetModel
        {
            Id = entity.Id,
            Email = entity.Email,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Color = entity.Color,
            Location = entity.Location,
            Bio = entity.Bio,
            Role = entity.Role.ToModel()
        };

        internal static UserProfileEntity ToEntity(this UserProfileGeneralInfoUpdateModel model, string id) => new UserProfileEntity
        {
            Id = id,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Color = model.Color,
            Location = model.Location,
            Bio = model.Bio,
        };
    }
}