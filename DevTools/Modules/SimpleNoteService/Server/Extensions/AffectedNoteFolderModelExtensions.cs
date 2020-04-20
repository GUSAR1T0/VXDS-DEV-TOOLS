using VXDesign.Store.DevTools.Common.Core.Entities.NoteFolder;
using VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Models.NoteFolder;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Extensions
{
    internal static class AffectedNoteFolderModelExtensions
    {
        internal static AffectedNoteFolderModel ToModel(this AffectedNoteFolderEntity entity) => new AffectedNoteFolderModel
        {
            FoldersCount = entity.FoldersCount,
            NotesCount = entity.NotesCount
        };
    }
}