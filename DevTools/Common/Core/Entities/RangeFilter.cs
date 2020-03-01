namespace VXDesign.Store.DevTools.Common.Core.Entities
{
    public class RangeFilter<T>
    {
        public T Min { get; set; }
        public T Max { get; set; }

        public bool HasRange => Min != null && Max != null;
    }
}