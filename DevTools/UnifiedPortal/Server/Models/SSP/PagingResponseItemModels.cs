using VXDesign.Store.DevTools.Core.Entities.Storage.SSP;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.SSP
{
    public interface IPagingResponseItemModel<out TModel, in TEntity> where TModel : PagingResponseItemModel where TEntity : IPagingResponseItemEntity
    {
        TModel ToModel(TEntity entity);
    }

    public abstract class PagingResponseItemModel
    {
    }
}