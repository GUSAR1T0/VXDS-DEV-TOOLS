using System.Collections.Generic;

namespace VXDesign.Store.DevTools.Core.Entities.Storage.SSP
{
    public interface IServerSidePagingResponse
    {
    }

    public abstract class ServerSidePagingResponse<TResponseItem> : IServerSidePagingResponse where TResponseItem : IPagingResponseItemEntity
    {
        public long Total { get; set; }
        public IEnumerable<TResponseItem> Items { get; set; }
    }
}