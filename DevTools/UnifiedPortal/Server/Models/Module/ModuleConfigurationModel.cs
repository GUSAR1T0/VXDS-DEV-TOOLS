using VXDesign.Store.DevTools.Common.Core.Controllers.Models.Common;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Module
{
    public class ModuleConfigurationModel
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public int FileId { get; set; }
        public FileModel File { get; set; }
    }
}