using VXDesign.Store.DevTools.Common.Core.Entities.Module;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Module;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions
{
    internal static class ModuleModelsExtensions
    {
        internal static ModuleConfigurationFileUploadResultModel ToModel(this ModuleConfigurationFileUploadResult entity) => new ModuleConfigurationFileUploadResultModel
        {
            ModuleId = entity.ModuleId,
            FileId = entity.FileId,
            Alias = entity.Alias,
            OldName = entity.OldName,
            NewName = entity.NewName,
            OldVersion = entity.OldVersion,
            NewVersion = entity.NewVersion,
            UserId = entity.UserId,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            HostId = entity.HostId,
            HostName = entity.HostName,
            HostDomain = entity.HostDomain,
            HostOperatingSystem = entity.HostOperatingSystem,
            OperatingSystems = entity.OperatingSystems,
            Verdict = entity.Verdict
        };

        internal static ModuleConfigurationSubmitEntity ToEntity(this ModuleConfigurationSubmitModel model) => new ModuleConfigurationSubmitEntity
        {
            ModuleId = model.ModuleId,
            FileId = model.FileId,
            UserId = model.UserId,
            HostId = model.HostId
        };
    }
}