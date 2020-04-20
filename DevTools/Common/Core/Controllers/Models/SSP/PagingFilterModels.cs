using VXDesign.Store.DevTools.Common.Core.Entities.SSP;

namespace VXDesign.Store.DevTools.Common.Core.Controllers.Models.SSP
{
    public interface IPagingFilterModel<out TEntity> where TEntity : IPagingFilterEntity
    {
        TEntity ToEntity();
    }

    public abstract class PagingFilterModel
    {
    }
}