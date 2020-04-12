namespace VXDesign.Store.DevTools.Common.Core.Entities.Note
{
    public class NoteProjectEntity : IDataEntity
    {
        public int Id { get; set; }
        public int NoteId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectAlias { get; set; }
    }
}