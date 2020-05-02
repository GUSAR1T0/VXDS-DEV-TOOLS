using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.Entities.Settings;
using VXDesign.Store.DevTools.Common.Core.Entities.SSP;

namespace VXDesign.Store.DevTools.Common.Core.Entities.Module
{
    public class ModuleEntity : IDataEntity, IPagingResponseItemEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Version { get; set; }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Color { get; set; }

        public int HostId { get; set; }
        public string HostName { get; set; }
        public string HostDomain { get; set; }
        public HostOperatingSystem HostOperatingSystem { get; set; }

        public bool IsActive { get; set; }
    }

    public class ModulePagingFilter : IPagingFilterEntity
    {
        public IEnumerable<int> Ids { get; set; }
        public IEnumerable<string> Names { get; set; }
        public IEnumerable<string> Aliases { get; set; }
        public IEnumerable<int> UserIds { get; set; }
        public IEnumerable<int> HostIds { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ModulePagingRequest : ServerSidePagingRequest<ModulePagingFilter>
    {
    }

    public class ModulePagingResponse : ServerSidePagingResponse<ModuleEntity>
    {
    }

    public class ModuleInfoEntity : IDataEntity
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
    }

    public class ModuleConfigurationEntity : IDataEntity
    {
        public int Id { get; set; }
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
    }
}