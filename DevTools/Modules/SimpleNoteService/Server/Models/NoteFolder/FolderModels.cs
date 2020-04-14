using System.Collections.Generic;

namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Models.NoteFolder
{
    public class FolderModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<FolderModel> Nodes { get; set; }
    }

    public class FolderShortModel
    {
        public int Id { get; set; }
        public IEnumerable<FolderShortModel> Nodes { get; set; }
    }
}