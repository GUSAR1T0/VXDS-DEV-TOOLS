using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using VXDesign.Store.DevTools.Common.Entities.Exceptions;

namespace VXDesign.Store.DevTools.Common.Entities.Operations
{
    public interface IOperationConnection : IDisposable
    {
        Task<int> ExecuteAsync(string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<int> ExecuteAsync(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<IEnumerable<T>> QueryAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<IEnumerable<T>> QueryAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QueryFirstAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QueryFirstAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QueryFirstOrDefaultAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QueryFirstOrDefaultAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QuerySingleAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QuerySingleAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QuerySingleOrDefaultAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null);
        Task<T> QuerySingleOrDefaultAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null);
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

        public async Task<int> ExecuteAsync(string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            VerifyOperationStatus();
            return await connection.ExecuteAsync(command, commandTimeout: commandTimeout, commandType: commandType, transaction: transaction);
        }

        public async Task<int> ExecuteAsync(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            VerifyOperationStatus();
            return await connection.ExecuteAsync(command, parameters, commandTimeout: commandTimeout, commandType: commandType, transaction: transaction);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            VerifyOperationStatus();
            return await connection.QueryAsync<T>(command, commandTimeout: commandTimeout, commandType: commandType, transaction: transaction);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            VerifyOperationStatus();
            return await connection.QueryAsync<T>(command, parameters, commandTimeout: commandTimeout, commandType: commandType, transaction: transaction);
        }

        public async Task<T> QueryFirstAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            VerifyOperationStatus();
            return await connection.QueryFirstAsync<T>(command, commandTimeout: commandTimeout, commandType: commandType, transaction: transaction);
        }

        public async Task<T> QueryFirstAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            VerifyOperationStatus();
            return await connection.QueryFirstAsync<T>(command, parameters, commandTimeout: commandTimeout, commandType: commandType, transaction: transaction);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            VerifyOperationStatus();
            return await connection.QueryFirstOrDefaultAsync<T>(command, commandTimeout: commandTimeout, commandType: commandType, transaction: transaction);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            VerifyOperationStatus();
            return await connection.QueryFirstOrDefaultAsync<T>(command, parameters, commandTimeout: commandTimeout, commandType: commandType, transaction: transaction);
        }

        public async Task<T> QuerySingleAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            VerifyOperationStatus();
            return await connection.QuerySingleAsync<T>(command, commandTimeout: commandTimeout, commandType: commandType, transaction: transaction);
        }

        public async Task<T> QuerySingleAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            VerifyOperationStatus();
            return await connection.QuerySingleAsync<T>(command, parameters, commandTimeout: commandTimeout, commandType: commandType, transaction: transaction);
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            VerifyOperationStatus();
            return await connection.QuerySingleOrDefaultAsync<T>(command, commandTimeout: commandTimeout, commandType: commandType, transaction: transaction);
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            VerifyOperationStatus();
            return await connection.QuerySingleOrDefaultAsync<T>(command, parameters, commandTimeout: commandTimeout, commandType: commandType, transaction: transaction);
        }

        public void Dispose()
        {
            transaction?.Dispose();
            connection?.Dispose();
        }
    }
}