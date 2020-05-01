using System.Collections.Generic;
using System.Linq;
using VXDesign.Store.DevTools.Common.Core.Controllers.Models.SSP;
using VXDesign.Store.DevTools.Common.Core.Entities.Settings;
using VXDesign.Store.DevTools.UnifiedPortal.Server.Extensions;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.Settings
{
    public class HostSettingsItemModel : PagingResponseItemModel, IPagingResponseItemModel<HostSettingsItemModel, HostSettingsItemEntity>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public HostOperationSystem OperationSystem { get; set; }
        public IEnumerable<HostCredentialsItemModel> Credentials { get; set; }

        public HostSettingsItemModel ToModel(HostSettingsItemEntity entity) => entity.ToModel();
    }

    public class HostCredentialsItemModel
    {
        public HostConnectionType Type { get; set; }
        public int? Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class HostPagingFilterModel : PagingFilterModel, IPagingFilterModel<HostPagingFilter>
    {
        public IEnumerable<int> Ids { get; set; }
        public IEnumerable<string> Names { get; set; }
        public IEnumerable<string> Domains { get; set; }
        public IEnumerable<HostOperationSystem> OperationSystems { get; set; }

        public HostPagingFilter ToEntity() => new HostPagingFilter
        {
            Ids = Ids,
            Names = Names,
            Domains = Domains,
            OperationSystems = OperationSystems
        };
    }

    public class HostPagingRequestModel : ServerSidePagingRequestModel<HostPagingFilterModel, HostPagingRequest>
    {
        public override HostPagingRequest ToEntity() => new HostPagingRequest
        {
            PageNo = PageNo,
            PageSize = PageSize,
            Filter = Filter.ToEntity()
        };
    }

    public class HostPagingResponseModel : ServerSidePagingResponseModel<HostSettingsItemModel, HostPagingResponseModel, HostPagingResponse>
    {
        public override HostPagingResponseModel ToModel(HostPagingResponse entity)
        {
            Total = entity.Total;
            Items = entity.Items.Select(item => new HostSettingsItemModel().ToModel(item));
            return this;
        }
    }

    public class HostSettingsShortModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
    }
}