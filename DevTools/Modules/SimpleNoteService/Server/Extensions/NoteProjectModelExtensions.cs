using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Entities.NoteFolder;
using VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Models.NoteFolder;

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