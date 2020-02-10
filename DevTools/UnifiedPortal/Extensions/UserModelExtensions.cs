using VXDesign.Store.DevTools.Core.Entities.Storage.User;
using VXDesign.Store.DevTools.UnifiedPortal.Models.User;

namespace VXDesign.Store.DevTools.UnifiedPortal.Extensions
{
    internal static class UserModelExtensions
    {
        internal static UserModel ToModel(this UserListItem entity) => new UserModel
        {
            Id = entity.Id,
            Email = entity.Email,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Color = entity.Color,
            UserRole = entity.UserRole,
            IsActivated = entity.IsActivated
        };

        internal static UserShortModel ToModel(this UserShortEntity entity) => new UserShortModel
        {
            Id = entity.Id,
            FullName = entity.FullName
        };

        internal static UserProfileGetModel ToModel(this UserProfileEntity entity) => new UserProfileGetModel
        {
            Id = entity.Id,
            Email = entity.Email,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Color = entity.Color,
            Location = entity.Location,
            Bio = entity.Bio,
            UserRole = entity.UserRole?.ToFullInfoModel(),
            IsActivated = entity.IsActivated
        };

        internal static UserProfileEntity ToEntity(this UserProfileGeneralInfoUpdateModel model, int id) => new UserProfileEntity
        {
            Id = id,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Color = model.Color,
            Location = model.Location,
            Bio = model.Bio
        };

        internal static UserProfileEntity ToEntity(this UserProfileAccountSpecificInfoUpdateModel model, int id) => new UserProfileEntity
        {
            Id = id,
            UserRoleId = model.UserRoleId,
            IsActivated = model.IsActivated
        };
    }
}