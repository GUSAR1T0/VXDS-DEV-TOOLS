namespace VXDesign.Store.DevTools.Modules.SimpleNoteService.Server.Models.NoteFolder
{
    public class NoteProjectModel
    {
        public int Id { get; set; }
        public int NoteId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectAlias { get; set; }
    }
}