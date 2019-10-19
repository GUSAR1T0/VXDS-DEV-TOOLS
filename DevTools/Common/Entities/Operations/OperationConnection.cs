using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

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
        private readonly SqlConnection connection;

        public OperationConnection(string dataStoreConnectionString)
        {
            connection = new SqlConnection(dataStoreConnectionString);
            connection.Open();
        }

        public async Task<int> ExecuteAsync(string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await connection.ExecuteAsync(command, commandTimeout: commandTimeout, commandType: commandType);
        }

        public async Task<int> ExecuteAsync(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await connection.ExecuteAsync(command, parameters, commandTimeout: commandTimeout, commandType: commandType);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await connection.QueryAsync<T>(command, commandTimeout: commandTimeout, commandType: commandType);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await connection.QueryAsync<T>(command, parameters, commandTimeout: commandTimeout, commandType: commandType);
        }

        public async Task<T> QueryFirstAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await connection.QueryFirstAsync<T>(command, commandTimeout: commandTimeout, commandType: commandType);
        }

        public async Task<T> QueryFirstAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await connection.QueryFirstAsync<T>(command, parameters, commandTimeout: commandTimeout, commandType: commandType);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await connection.QueryFirstOrDefaultAsync<T>(command, commandTimeout: commandTimeout, commandType: commandType);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await connection.QueryFirstOrDefaultAsync<T>(command, parameters, commandTimeout: commandTimeout, commandType: commandType);
        }

        public async Task<T> QuerySingleAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await connection.QuerySingleAsync<T>(command, commandTimeout: commandTimeout, commandType: commandType);
        }

        public async Task<T> QuerySingleAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await connection.QuerySingleAsync<T>(command, parameters, commandTimeout: commandTimeout, commandType: commandType);
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await connection.QuerySingleOrDefaultAsync<T>(command, commandTimeout: commandTimeout, commandType: commandType);
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(object parameters, string command, CommandType? commandType = null, int? commandTimeout = null)
        {
            return await connection.QuerySingleOrDefaultAsync<T>(command, parameters, commandTimeout: commandTimeout, commandType: commandType);
        }

        public void Dispose()
        {
            connection?.Dispose();
        }
    }
}