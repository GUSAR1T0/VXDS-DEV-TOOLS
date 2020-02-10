using VXDesign.Store.DevTools.Core.Entities.Storage.SSP;

namespace VXDesign.Store.DevTools.UnifiedPortal.Models.SSP
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