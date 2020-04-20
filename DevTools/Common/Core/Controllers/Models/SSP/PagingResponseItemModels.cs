using VXDesign.Store.DevTools.Common.Core.Entities.SSP;

namespace VXDesign.Store.DevTools.Common.Core.Controllers.Models.SSP
{
    public interface IPagingResponseItemModel<out TModel, in TEntity> where TModel : PagingResponseItemModel where TEntity : IPagingResponseItemEntity
    {
        TModel ToModel(TEntity entity);
    }

    public abstract class PagingResponseItemModel
    {
    }
}