using VXDesign.Store.DevTools.Core.Entities.Storage;
using VXDesign.Store.DevTools.UnifiedPortal.Models.Common;

namespace VXDesign.Store.DevTools.UnifiedPortal.Extensions
{
    internal static class RangeFilterModelExtensions
    {
        internal static RangeFilter<T> ToEntity<T>(this RangeFilterModel<T> model) => model != null ? new RangeFilter<T>
        {
            Min = model.Min,
            Max = model.Max
        } : null;
    }
}