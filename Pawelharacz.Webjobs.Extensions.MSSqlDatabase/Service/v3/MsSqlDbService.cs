﻿#if NETCOREAPP3_1
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Extensions;

namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Service.v3
{
    public class MsSqlDbService: IMsSqlDbService
    {
        private readonly string _connectionString;

        public MsSqlDbService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<T> Get<T>(MsSqlSpec msSqlSpec) where T : class, new()
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(msSqlSpec.Query, connection);
            foreach (var sqlParameter in msSqlSpec.Parameters)
            {
                command.Parameters.Add(sqlParameter);
            }
            connection.Open();
            var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            while (reader.Read())
            {
                yield return reader.ConvertToObject<T>();
            }
        }

        public async IAsyncEnumerable<T> GetAsync<T>(MsSqlSpec msSqlSpec, [EnumeratorCancellation] CancellationToken cancellationToken = default) where T : class, new()
        {
            await using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(msSqlSpec.Query, connection);
            foreach (var sqlParameter in msSqlSpec.Parameters)
            {
                command.Parameters.Add(sqlParameter);
            }
            await connection.OpenAsync(cancellationToken);
            var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                yield return reader.ConvertToObject<T>();
            }
        }

        public async IAsyncEnumerable<object> GetAsync(MsSqlSpec msSqlSpec, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(msSqlSpec.Query, connection);
            foreach (var sqlParameter in msSqlSpec.Parameters)
            {
                command.Parameters.Add(sqlParameter);
            }
            await connection.OpenAsync(cancellationToken);
            var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                yield return reader.ConvertToObject();
            }
        }

        public async Task<T> GetOne<T>(MsSqlSpec msSqlSpec, CancellationToken cancellationToken = default) where T : class, new()
        {
            await using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand(msSqlSpec.Query, connection);
            foreach (var sqlParameter in msSqlSpec.Parameters)
            {
                command.Parameters.Add(sqlParameter);
            }
            await connection.OpenAsync(cancellationToken);
            var reader =await command.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken);
            var isRead = await reader.ReadAsync(cancellationToken);
            if (isRead)
            {
                return reader.ConvertToObject<T>();
            }
            return default(T);
        }
        
    }
}
#endif