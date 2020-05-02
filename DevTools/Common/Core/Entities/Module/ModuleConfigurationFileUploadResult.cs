namespace VXDesign.Store.DevTools.Common.Core.Entities.Module
{
    public class ModuleConfigurationFileUploadResult
    {
        public int? Id { get; set; }
        public int FileId { get; set; }
        public string Alias { get; set; }
        public string OldName { get; set; }
        public string NewName { get; set; }
        public string OldVersion { get; set; }
        public string NewVersion { get; set; }
        public ModuleConfigurationVerdict Verdict { get; set; }
    }
}