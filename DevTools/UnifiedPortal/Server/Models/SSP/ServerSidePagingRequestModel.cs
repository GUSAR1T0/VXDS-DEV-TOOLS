using VXDesign.Store.DevTools.Common.Core.Entities.SSP;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.SSP
{
    public abstract class ServerSidePagingRequestModel<TFilterModel, TRequest>
        where TFilterModel : PagingFilterModel
        where TRequest : IServerSidePagingRequest, new()
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public TFilterModel Filter { get; set; }

        public abstract TRequest ToEntity();
    }
}