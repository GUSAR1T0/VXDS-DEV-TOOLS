using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using VXDesign.Store.DevTools.Core.Entities.Exceptions;

namespace VXDesign.Store.DevTools.Core.Entities.Operations
{
    public interface IOperationConnection : IDisposable
    {
        #region ExecuteAsync

        Task<int> ExecuteAsync(string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<int> ExecuteAsync(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<int> ExecuteAsync(Action<DynamicParameters> parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<int> ExecuteAsync(DynamicParameters parameters, string command, CommandType? commandType = null, int? commandTimeout = null);

        #endregion

        #region QueryAsync

        Task<IEnumerable<T>> QueryAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<IEnumerable<T>> QueryAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<IEnumerable<T>> QueryAsync<T>(Action<DynamicParameters> parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<IEnumerable<T>> QueryAsync<T>(DynamicParameters parameters, string command, CommandType? commandType = null, int? commandTimeout = null);

        #endregion

        #region QueryFirstAsync

        Task<T> QueryFirstAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QueryFirstAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QueryFirstAsync<T>(Action<DynamicParameters> parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QueryFirstAsync<T>(DynamicParameters parameters, string command, CommandType? commandType = null, int? commandTimeout = null);

        #endregion

        #region QueryFirstOrDefaultAsync

        Task<T> QueryFirstOrDefaultAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QueryFirstOrDefaultAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QueryFirstOrDefaultAsync<T>(Action<DynamicParameters> parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QueryFirstOrDefaultAsync<T>(DynamicParameters parameters, string command, CommandType? commandType = null, int? commandTimeout = null);

        #endregion

        #region QuerySingleAsync

        Task<T> QuerySingleAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QuerySingleAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QuerySingleAsync<T>(Action<DynamicParameters> parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QuerySingleAsync<T>(DynamicParameters parameters, string command, CommandType? commandType = null, int? commandTimeout = null);

        #endregion

        #region QuerySingleOrDefaultAsync

        Task<T> QuerySingleOrDefaultAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QuerySingleOrDefaultAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QuerySingleOrDefaultAsync<T>(Action<DynamicParameters> parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QuerySingleOrDefaultAsync<T>(DynamicParameters parameters, string command, CommandType? commandType = null, int? commandTimeout = null);

        #endregion

        #region QueryMultipleAsync

        Task<SqlMapper.GridReader> QueryMultipleAsync(string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<SqlMapper.GridReader> QueryMultipleAsync(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<SqlMapper.GridReader> QueryMultipleAsync(Action<DynamicParameters> parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<SqlMapper.GridReader> QueryMultipleAsync(DynamicParameters parameters, string command, CommandType? commandType = null, int? commandTimeout = null);

        #endregion
    }

    public class OperationConnection : IOperationConnection
    {
        private readonly IOperation operation;
        private readonly SqlConnection connection;
        private SqlTransaction transaction;

        public OperationConnection(IOperation operation, string dataStoreConnectionString)
        {
            this.operation = operation;
            connection = new SqlConnection(dataStoreConnectionString);
            connection.Open();
        }

        internal SqlTransaction BeginTransaction()
        {
            if (transaction != null)
            {
                throw CommonExceptions.TransactionHasAlreadyBegun(operation);
            }

            return transaction = connection.BeginTransaction(IsolationLevel.Snapshot);
        }

        internal void EndTransaction()
        {
            transaction?.Dispose();
            transaction = null;
        }

        private void VerifyOperationStatus()
        {
            if (operation?.IsSuccessful != null)
            {
                throw CommonExceptions.OperationHasAlreadyCompleted(operation);
            }
        }

        private async Task<T> Execute<T>(Func<string, DynamicParameters, SqlTransaction, int?, CommandType?, Task<T>> function, DynamicParameters parameters, string command,
            CommandType? commandType = null, int? commandTimeout = null)
        {
            VerifyOperationStatus();
            if (operation.OperationId != -1)
            {
                parameters.Add("@OperationId", operation.OperationId);
            }

            return await function(command, parameters, transaction, commandTimeout, commandType);
        }

        #region ExecuteAsync

        public async Task<int> ExecuteAsync(string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await ExecuteAsync(new DynamicParameters(), command, commandType, commandTimeout);
        }

        public async Task<int> ExecuteAsync(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await ExecuteAsync(new DynamicParameters(parameters), command, commandType, commandTimeout);
        }

        public async Task<int> ExecuteAsync(Action<DynamicParameters> parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            var dp = new DynamicParameters();
            parameters(dp);
            return await ExecuteAsync(dp, command, commandType, commandTimeout);
        }

        public async Task<int> ExecuteAsync(DynamicParameters parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await Execute(connection.ExecuteAsync, parameters, command, commandType, commandTimeout);
        }

        #endregion

        #region QueryAsync

        public async Task<IEnumerable<T>> QueryAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await QueryAsync<T>(new DynamicParameters(), command, commandType, commandTimeout);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await QueryAsync<T>(new DynamicParameters(parameters), command, commandType, commandTimeout);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(Action<DynamicParameters> parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            var dp = new DynamicParameters();
            parameters(dp);
            return await QueryAsync<T>(dp, command, commandType, commandTimeout);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(DynamicParameters parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await Execute(connection.QueryAsync<T>, parameters, command, commandType, commandTimeout);
        }

        #endregion

        #region QueryFirstAsync

        public async Task<T> QueryFirstAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await QueryFirstAsync<T>(new DynamicParameters(), command, commandType, commandTimeout);
        }

        public async Task<T> QueryFirstAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await QueryFirstAsync<T>(new DynamicParameters(parameters), command, commandType, commandTimeout);
        }

        public async Task<T> QueryFirstAsync<T>(Action<DynamicParameters> parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            var dp = new DynamicParameters();
            parameters(dp);
            return await QueryFirstAsync<T>(dp, command, commandType, commandTimeout);
        }

        public async Task<T> QueryFirstAsync<T>(DynamicParameters parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await Execute(connection.QueryFirstAsync<T>, parameters, command, commandType, commandTimeout);
        }

        #endregion

        #region QueryFirstOrDefaultAsync

        public async Task<T> QueryFirstOrDefaultAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await QueryFirstOrDefaultAsync<T>(new DynamicParameters(), command, commandType, commandTimeout);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await QueryFirstOrDefaultAsync<T>(new DynamicParameters(parameters), command, commandType, commandTimeout);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(Action<DynamicParameters> parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            var dp = new DynamicParameters();
            parameters(dp);
            return await QueryFirstOrDefaultAsync<T>(dp, command, commandType, commandTimeout);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(DynamicParameters parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await Execute(connection.QueryFirstOrDefaultAsync<T>, parameters, command, commandType, commandTimeout);
        }

        #endregion

        #region QuerySingleAsync

        public async Task<T> QuerySingleAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await QuerySingleAsync<T>(new DynamicParameters(), command, commandType, commandTimeout);
        }

        public async Task<T> QuerySingleAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await QuerySingleAsync<T>(new DynamicParameters(parameters), command, commandType, commandTimeout);
        }

        public async Task<T> QuerySingleAsync<T>(Action<DynamicParameters> parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            var dp = new DynamicParameters();
            parameters(dp);
            return await QuerySingleAsync<T>(dp, command, commandType, commandTimeout);
        }

        public async Task<T> QuerySingleAsync<T>(DynamicParameters parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await Execute(connection.QuerySingleAsync<T>, parameters, command, commandType, commandTimeout);
        }

        #endregion

        #region QuerySingleOrDefaultAsync

        public async Task<T> QuerySingleOrDefaultAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await QuerySingleOrDefaultAsync<T>(new DynamicParameters(), command, commandType, commandTimeout);
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await QuerySingleOrDefaultAsync<T>(new DynamicParameters(parameters), command, commandType, commandTimeout);
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(Action<DynamicParameters> parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            var dp = new DynamicParameters();
            parameters(dp);
            return await QuerySingleOrDefaultAsync<T>(dp, command, commandType, commandTimeout);
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(DynamicParameters parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await Execute(connection.QuerySingleOrDefaultAsync<T>, parameters, command, commandType, commandTimeout);
        }

        #endregion

        #region QueryMultipleAsync

        public async Task<SqlMapper.GridReader> QueryMultipleAsync(string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await QueryMultipleAsync(new DynamicParameters(), command, commandType, commandTimeout);
        }

        public async Task<SqlMapper.GridReader> QueryMultipleAsync(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await QueryMultipleAsync(new DynamicParameters(parameters), command, commandType, commandTimeout);
        }

        public async Task<SqlMapper.GridReader> QueryMultipleAsync(Action<DynamicParameters> parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            var dp = new DynamicParameters();
            parameters(dp);
            return await QueryMultipleAsync(dp, command, commandType, commandTimeout);
        }

        public async Task<SqlMapper.GridReader> QueryMultipleAsync(DynamicParameters parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await Execute(connection.QueryMultipleAsync, parameters, command, commandType, commandTimeout);
        }

        #endregion

        public void Dispose()
        {
            transaction?.Dispose();
            connection?.Dispose();
        }
    }
}