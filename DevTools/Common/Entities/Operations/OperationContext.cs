using VXDesign.Store.DevTools.Common.Utils.Base;

namespace VXDesign.Store.DevTools.Common.Entities.Operations
{
    public class OperationContext
    {
        public string Name { get; private set; }

        public static OperationContext Create(string first, params string[] other) => new OperationContext
        {
            Name = string.Join(":", ListUtils.Initialize(first, other))
        };
    }
}