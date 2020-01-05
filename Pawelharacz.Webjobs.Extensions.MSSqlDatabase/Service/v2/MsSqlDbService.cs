using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Extensions;

#if !NETCOREAPP3_1
namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Service.v2
{

    public class MsSqlDbService : IMsSqlDbService
    {
        private readonly string _connectionString;

        public MsSqlDbService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<T> Get<T>(MsSqlSpec msSqlSpec) where T : class, new()
        {
            using( var connection = new SqlConnection(_connectionString))
            {
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
        }

        public async Task<IEnumerable<T>> GetAsync<T>(MsSqlSpec msSqlSpec,
            CancellationToken cancellationToken = default) where T : class, new()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(msSqlSpec.Query, connection);
                foreach (var sqlParameter in msSqlSpec.Parameters)
                {
                    command.Parameters.Add(sqlParameter);
                }

                await connection.OpenAsync(cancellationToken);
                var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken);
                var list = new List<T>();
                while (await reader.ReadAsync(cancellationToken))
                {
                    list.Add(reader.ConvertToObject<T>());
                }

                return list.AsEnumerable();
            }
        }

        public async Task<T> GetOne<T>(MsSqlSpec msSqlSpec, CancellationToken cancellationToken = default)
            where T : class, new()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(msSqlSpec.Query, connection);
                foreach (var sqlParameter in msSqlSpec.Parameters)
                {
                    command.Parameters.Add(sqlParameter);
                }

                await connection.OpenAsync(cancellationToken);
                var reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection, cancellationToken);
                var isRead = await reader.ReadAsync(cancellationToken);
                if (isRead)
                {
                    return reader.ConvertToObject<T>();
                }

                return default(T);
            }
        }
    }
}
#endif