using VXDesign.Store.DevTools.Common.DataStorage.Entities;
using VXDesign.Store.DevTools.UnifiedPortal.Models.User;

namespace VXDesign.Store.DevTools.UnifiedPortal.Extensions
{
    internal static class UserModelExtensions
    {
        internal static UserProfileModel ToModel(this UserProfileEntity entity) => new UserProfileModel
        {
            Id = entity.Id,
            Email = entity.Email,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Color = entity.Color,
            Location = entity.Location,
            Bio = entity.Bio
        };

        internal static UserProfileEntity ToEntity(this UserProfileModel model) => new UserProfileEntity
        {
            Id = model.Id,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Color = model.Color,
            Location = model.Location,
            Bio = model.Bio
        };
    }
}