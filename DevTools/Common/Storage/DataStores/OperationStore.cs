using System.Threading.Tasks;
using VXDesign.Store.DevTools.Common.Entities.Operations;

namespace VXDesign.Store.DevTools.Common.Storage.DataStores
{
    public interface IOperationStore
    {
        Task<int> Start(string scope, OperationContext context);
        Task Stop(int operationId, bool isSuccessful);
    }

    public sealed class OperationStore : BaseDataStore, IOperationStore
    {
        private readonly IOperationConnection connection;

        public OperationStore(IOperationConnection connection)
        {
            this.connection = connection;
        }

        public async Task<int> Start(string scope, OperationContext context)
        {
            return await connection.QueryFirstAsync<int>(new
            {
                Scope = scope,
                ContextName = context.Name,
                context.UserId,
                context.IsSystemAction
            }, @"
                DECLARE @Id TABLE ([Id] INT);
                INSERT INTO [base].[Operation] ([Scope], [ContextName], [UserId], [IsSystemAction])
                OUTPUT INSERTED.[Id] INTO @Id
                VALUES (@Scope, @ContextName, @UserId, @IsSystemAction);
                SELECT i.[Id] FROM @Id i;
            ");
        }

        public async Task Stop(int operationId, bool isSuccessful)
        {
            await connection.ExecuteAsync(new
            {
                Id = operationId,
                IsSuccessful = isSuccessful
            }, @"
                UPDATE [base].[Operation]
                SET [IsSuccessful] = @IsSuccessful
                WHERE [Id] = @Id;
            ");
        }
    }
}