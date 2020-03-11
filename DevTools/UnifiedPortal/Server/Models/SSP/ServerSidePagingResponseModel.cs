using System.Collections.Generic;
using VXDesign.Store.DevTools.Common.Core.Entities.SSP;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.SSP
{
    public abstract class ServerSidePagingResponseModel<TResponseItemModel, TResponseModel, TResponse>
        where TResponseItemModel : PagingResponseItemModel
        where TResponseModel : ServerSidePagingResponseModel<TResponseItemModel, TResponseModel, TResponse>
        where TResponse : IServerSidePagingResponse
    {
        public long Total { get; set; }
        public IEnumerable<TResponseItemModel> Items { get; set; }

        public abstract TResponseModel ToModel(TResponse entity);
    }
}