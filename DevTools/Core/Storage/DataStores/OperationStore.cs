using System.Threading.Tasks;
using VXDesign.Store.DevTools.Core.Entities.Operations;

namespace VXDesign.Store.DevTools.Core.Storage.DataStores
{
    public interface IOperationStore
    {
        Task<long> Start(string scope, OperationContext context);
        Task Stop(long operationId, bool isSuccessful);
    }

    public sealed class OperationStore : BaseDataStore, IOperationStore
    {
        private readonly IOperationConnection connection;

        public OperationStore(IOperationConnection connection)
        {
            this.connection = connection;
        }

        public async Task<long> Start(string scope, OperationContext context)
        {
            return await connection.QueryFirstAsync<long>(new
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
                    [StopDate] = SYSDATETIMEOFFSET()
                WHERE [Id] = @Id;
            ");
        }
    }
}