using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Entities.NoteFolder;
using VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Models.NoteFolder;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Extensions
{
    internal static class FolderModelsExtensions
    {
        internal static FolderModel ToModel(this FolderNode node) => new FolderModel
        {
            Id = node.Entity.Id,
            Name = node.Entity.Name,
            Nodes = node.Nodes.Select(item => item.ToModel())
        };

        internal static FolderShortNode ToEntity(this FolderShortModel node) => new FolderShortNode
        {
            Id = node.Id,
            Nodes = node.Nodes.Select(item => item.ToEntity())
        };
    }
}