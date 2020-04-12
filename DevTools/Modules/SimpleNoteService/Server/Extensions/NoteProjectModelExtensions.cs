using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Entities.Note;
using VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Models.Note;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Extensions
{
    internal static class NoteProjectModelExtensions
    {
        internal static IEnumerable<NoteProjectModel> ToModel(this IEnumerable<NoteProjectEntity> entities) => entities.Select(entity => new NoteProjectModel
        {
            Id = entity.Id,
            NoteId = entity.NoteId,
            ProjectId = entity.ProjectId,
            ProjectName = entity.ProjectName,
            ProjectAlias = entity.ProjectAlias
        });
    }
}