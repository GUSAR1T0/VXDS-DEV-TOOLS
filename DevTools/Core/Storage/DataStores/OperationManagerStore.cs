using System.Threading.Tasks;
using VXDesign.Store.DevTools.Core.Entities.Operations;

namespace VXDesign.Store.DevTools.Core.Storage.DataStores
{
    internal interface IOperationManagerStore
    {
        Task<long> Start(OperationContext context);
        Task Stop(long operationId, bool isSuccessful);
    }

    internal sealed class OperationManagerStore : BaseDataStore, IOperationManagerStore
    {
        private readonly IOperationConnection connection;

        public OperationManagerStore(IOperationConnection connection)
        {
            this.connection = connection;
        }

        public async Task<long> Start(OperationContext context)
        {
            return await connection.QueryFirstAsync<long>(new
            {
                context.Scope,
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

        public async Task Stop(long operationId, bool isSuccessful)
        {
            await connection.ExecuteAsync(new
            {
                Id = operationId,
                IsSuccessful = isSuccessful
            }, @"
                UPDATE [base].[Operation]
                SET
                    [IsSuccessful] = @IsSuccessful,
                    [StopTime] = SYSDATETIME()
                WHERE [Id] = @Id;
            ");
        }
    }
}