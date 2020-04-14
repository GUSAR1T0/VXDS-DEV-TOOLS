using VXDesign.Store.DevTools.Common.Core.Entities.NoteFolder;
using VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Models.NoteFolder;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Extensions
{
    internal static class NotePagingModelsExtensions
    {
        internal static NotePagingRequest ToEntity(this NotePagingRequestModel model, int folderId)
        {
            var entity = model.ToEntity();
            entity.Filter.FolderId = folderId;
            return entity;
        }
    }
}