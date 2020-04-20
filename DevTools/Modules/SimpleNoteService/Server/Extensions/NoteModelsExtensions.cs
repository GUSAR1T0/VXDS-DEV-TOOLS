using VXDesign.Store.DevTools.Common.Core.Constants;
using VXDesign.Store.DevTools.Common.Core.Entities.NoteFolder;
using VXDesign.Store.DevTools.Common.Core.Extensions;
using VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Models.NoteFolder;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Extensions
{
    internal static class NoteModelsExtensions
    {
        internal static NoteExtendedModel ToModel(this NoteExtendedEntity entity) => new NoteExtendedModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Text = entity.Text,
            UserId = entity.UserId,
            Color = entity.Color,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            EditTime = entity.EditTime.FormatDateTime(FormatPattern.FullDateTimeWithDayOfWeek),
            Projects = entity.Projects.ToModel(),
            FolderId = entity.FolderId,
            FolderName = entity.FolderName
        };

        internal static NoteUpdateEntity ToEntity(this NoteUpdateModel model) => new NoteUpdateEntity
        {
            Title = model.Title,
            Text = model.Text,
            ProjectIds = model.ProjectIds
        };
    }
}