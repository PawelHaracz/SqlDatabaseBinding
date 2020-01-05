using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Config;

namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Bindings
{
    internal class MsSqlDbAsyncConverter<T> : IAsyncConverter<MsSqlDbAttribute, T> where T : class, new()
    {
        private readonly MsSqlDbExtensionConfigProvider _msSqlDbExtensionConfigProvider;

        public MsSqlDbAsyncConverter(MsSqlDbExtensionConfigProvider msSqlDbExtensionConfigProvider)
        {
            _msSqlDbExtensionConfigProvider = msSqlDbExtensionConfigProvider;
        }
        public Task<T> ConvertAsync(MsSqlDbAttribute input, CancellationToken cancellationToken)
        {
            var context = _msSqlDbExtensionConfigProvider.CreateContext(input);

            var sqlSpec = new MsSqlSpec()
            {
                Parameters = input.SqlQueryParameters,
                Query = input.SqlQuery
            };

            return context.MsSqlDbService.GetOne<T>(sqlSpec, cancellationToken);
        }
    }
}