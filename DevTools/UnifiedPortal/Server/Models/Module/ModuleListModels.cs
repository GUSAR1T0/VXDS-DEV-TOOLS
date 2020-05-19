using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Controllers.Models.SSP;
using VXDesign.Store.DevTools.Common.Core.Entities.Module;
using VXDesign.Store.DevTools.Common.Core.Entities.Settings;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Module
{
    public class ModuleListItemModel : PagingResponseItemModel, IPagingResponseItemModel<ModuleListItemModel, ModuleListItemEntity>
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

        public ModuleStatus Status { get; set; }

        public ModuleListItemModel ToModel(ModuleListItemEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Alias = entity.Alias;
            Version = entity.Version;
            UserId = entity.UserId;
            FirstName = entity.FirstName;
            LastName = entity.LastName;
            Color = entity.Color;
            HostId = entity.HostId;
            HostName = entity.HostName;
            HostDomain = entity.HostDomain;
            HostOperatingSystem = entity.HostOperatingSystem;
            Status = entity.Status;
            return this;
        }
    }

    public class ModulePagingFilterModel : PagingFilterModel, IPagingFilterModel<ModulePagingFilter>
    {
        public IEnumerable<int> Ids { get; set; }
        public IEnumerable<string> Names { get; set; }
        public IEnumerable<string> Aliases { get; set; }
        public IEnumerable<int> UserIds { get; set; }
        public IEnumerable<int> HostIds { get; set; }
        public IEnumerable<ModuleStatus> Statuses { get; set; }

        public ModulePagingFilter ToEntity() => new ModulePagingFilter
        {
            Ids = Ids,
            Names = Names,
            Aliases = Aliases,
            UserIds = UserIds,
            HostIds = HostIds,
            Statuses = Statuses
        };
    }

    public class ModulePagingRequestModel : ServerSidePagingRequestModel<ModulePagingFilterModel, ModulePagingRequest>
    {
        public override ModulePagingRequest ToEntity() => new ModulePagingRequest
        {
            PageNo = PageNo,
            PageSize = PageSize,
            Filter = Filter.ToEntity()
        };
    }

    public class ModulePagingResponseModel : ServerSidePagingResponseModel<ModuleListItemModel, ModulePagingResponseModel, ModulePagingResponse>
    {
        public override ModulePagingResponseModel ToModel(ModulePagingResponse entity)
        {
            Total = entity.Total;
            Items = entity.Items.Select(item => new ModuleListItemModel().ToModel(item));
            return this;
        }
    }
}