using VXDesign.Store.DevTools.Core.Utils.Base;

namespace VXDesign.Store.DevTools.Core.Entities.Operations
{
    public class OperationContext
    {
        public class OperationContextBuilder
        {
            private readonly OperationContext operationContext;

            internal OperationContextBuilder()
            {
                operationContext = new OperationContext();
            }

            public OperationContextBuilder SetName(string first, params string[] other)
            {
                operationContext.Name = string.Join(":", ListUtils.Initialize(first, other));
                return this;
            }

            public OperationContextBuilder SetUserId(int? userId, bool isSystemAction = false)
            {
                operationContext.UserId = userId;
                operationContext.IsSystemAction = isSystemAction;
                return this;
            }

            public OperationContext Create() => operationContext;
        }

        private OperationContext()
        {
        }

        public string Name { get; private set; }
        public int? UserId { get; private set; }
        public bool IsSystemAction { get; private set; }

        public static OperationContextBuilder Builder() => new OperationContextBuilder();
    }
}