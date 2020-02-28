using VXDesign.Store.DevTools.Core.Entities.Storage.SSP;

namespace VXDesign.Store.DevTools.UnifiedPortal.Server.Models.SSP
{
    public interface IPagingFilterModel<out TEntity> where TEntity : IPagingFilterEntity
    {
        TEntity ToEntity();
    }

    public abstract class PagingFilterModel
    {
    }
}