using VXDesign.Store.DevTools.Common.Core.Entities.File;

namespace VXDesign.Store.DevTools.Common.Core.Controllers.Models.Common
{
    public class FileModel
    {
        public string Name { get; set; }
        public FileExtension? Extension { get; set; }
        public string Content { get; set; }
        public string Time { get; set; }
    }
}