using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Entities.Settings;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Settings;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions
{
    internal static class SettingsParametersModelExtensions
    {
        internal static HostSettingsItemEntity ToEntity(this HostSettingsItemModel model, int? id = null) => new HostSettingsItemEntity
        {
            Id = id ?? model.Id,
            Name = model.Name,
            Domain = model.Domain,
            OperatingSystem = model.OperatingSystem,
            CredentialsList = model.Credentials?.Select(item => new HostCredentialsItemEntity
            {
                Type = item.Type,
                Port = item.Port > 0 ? item.Port : null,
                Username = item.Username,
                Password = item.Password
            }) ?? new List<HostCredentialsItemEntity>()
        };

        internal static HostSettingsShortModel ToModel(this HostSettingsShortEntity entity) => new HostSettingsShortModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Domain = entity.Domain,
            OperatingSystem = entity.OperatingSystem
        };

        internal static HostSettingsItemModel ToModel(this HostSettingsItemEntity entity) => new HostSettingsItemModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Domain = entity.Domain,
            OperatingSystem = entity.OperatingSystem,
            Credentials = entity.CredentialsList?.Select(item => new HostCredentialsItemModel
            {
                Type = item.Type,
                Port = item.Port,
                Username = item.Username,
                Password = item.Password
            }) ?? new List<HostCredentialsItemModel>()
        };

        internal static CodeServicesSettingsModel ToModel(this CodeServiceSettingsEntity entity) => new CodeServicesSettingsModel
        {
            GitHubUser = entity.GitHubUser.ToModel()
        };
    }
}