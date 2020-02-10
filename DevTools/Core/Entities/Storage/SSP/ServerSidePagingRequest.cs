namespace VXDesign.Store.DevTools.Core.Entities.Storage.SSP
{
    public interface IServerSidePagingRequest
    {
    }

    public abstract class ServerSidePagingRequest<TFilter> : IServerSidePagingRequest where TFilter : IPagingFilterEntity
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public TFilter Filter { get; set; }
    }
}