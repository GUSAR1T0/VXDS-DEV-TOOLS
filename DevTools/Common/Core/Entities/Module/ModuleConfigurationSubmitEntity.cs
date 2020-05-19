namespace VXDesign.Store.DevTools.Common.Core.Entities.Module
{
    public class ModuleConfigurationSubmitEntity
    {
        public int? ModuleId { get; set; }
        public int FileId { get; set; }
        public int UserId { get; set; }
        public int HostId { get; set; }
    }
}