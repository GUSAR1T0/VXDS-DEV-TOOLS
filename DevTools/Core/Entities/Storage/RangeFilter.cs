namespace VXDesign.Store.DevTools.Core.Entities.Storage
{
    public class RangeFilter<T>
    {
        public T Min { get; set; }
        public T Max { get; set; }

        public bool HasRange => Min != null && Max != null;
    }
}