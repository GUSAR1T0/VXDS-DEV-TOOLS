using VXDesign.Store.DevTools.Common.Core.Entities.Module;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Module;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions
{
    internal static class ModuleModelsExtensions
    {
        internal static ModuleConfigurationFileUploadResultModel ToModel(this ModuleConfigurationFileUploadResult entity) => new ModuleConfigurationFileUploadResultModel
        {
            Id = entity.Id,
            FileId = entity.FileId,
            Alias = entity.Alias,
            OldName = entity.OldName,
            NewName = entity.NewName,
            OldVersion = entity.OldVersion,
            NewVersion = entity.NewVersion,
            Verdict = entity.Verdict
        };
    }
}