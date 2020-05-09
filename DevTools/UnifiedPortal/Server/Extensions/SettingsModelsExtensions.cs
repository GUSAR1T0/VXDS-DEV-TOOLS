using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Clients.RemoteHost.Entities;
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

        internal static HostCredentialsItemModel ToModel(this HostCredentialsItemEntity entity) => new HostCredentialsItemModel
        {
            Type = entity.Type,
            Port = entity.Port,
            Username = entity.Username,
            Password = entity.Password
        };

        internal static HostSettingsShortModel ToModel(this HostSettingsEntity entity) => new HostSettingsShortModel
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
            Credentials = entity.CredentialsList?.Select(item => item.ToModel()) ?? new List<HostCredentialsItemModel>()
        };

        internal static CheckConnectionsToHostResultModel ToModel(this (HostCredentialsItemEntity entity, CommandResult result) tuple) => new CheckConnectionsToHostResultModel
        {
            Item = tuple.entity.ToModel(),
            Result = tuple.result
        };

        internal static IEnumerable<CheckConnectionsToHostResultModel> ToModels(this IDictionary<HostCredentialsItemEntity, CommandResult> result) => result
            .Select(item => new CheckConnectionsToHostResultModel
            {
                Item = item.Key.ToModel(),
                Result = item.Value
            });

        internal static CheckConnectionToHostEntity ToEntity(this CheckConnectionToHostModel model) => new CheckConnectionToHostEntity
        {
            OperatingSystem = model.OperatingSystem,
            Host = model.Host,
            Type = model.Type,
            Port = model.Port > 0 ? model.Port : null,
            Username = model.Username,
            Password = model.Password
        };

        internal static CodeServicesSettingsModel ToModel(this CodeServiceSettingsEntity entity) => new CodeServicesSettingsModel
        {
            GitHubUser = entity.GitHubUser.ToModel()
        };
    }
}