using VXDesign.Store.DevTools.Common.DataStorage.Entities;
using VXDesign.Store.DevTools.UnifiedPortal.Models.User;

namespace VXDesign.Store.DevTools.UnifiedPortal.Extensions
{
    internal static class UserModelExtensions
    {
        internal static FullUserDataModel ToModel(this FullUserDataEntity entity) => new FullUserDataModel
        {
            Email = entity.Email,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Color = entity.Color
        };
    }
}