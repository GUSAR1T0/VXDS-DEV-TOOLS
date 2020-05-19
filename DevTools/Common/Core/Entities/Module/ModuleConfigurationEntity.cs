using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.Entities.Settings;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Module
{
    public class ModuleConfigurationEntity
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public IEnumerable<HostOperatingSystem> OperatingSystems { get; set; }
        public int FileId { get; set; }
        public File.File File { get; set; }
    }
}