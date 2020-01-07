using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Config;

namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Bindings
{
    internal class MsSqlDbDynamicAsyncConverter : IAsyncConverter<MsSqlDbAttribute, IEnumerable<object>>
    {
        private readonly MsSqlDbExtensionConfigProvider _msSqlDbExtensionConfigProvider;
        
        public MsSqlDbDynamicAsyncConverter(MsSqlDbExtensionConfigProvider msSqlDbExtensionConfigProvider)
        {
            _msSqlDbExtensionConfigProvider = msSqlDbExtensionConfigProvider;
        }
        public async Task<IEnumerable<dynamic>> ConvertAsync(MsSqlDbAttribute input, CancellationToken cancellationToken)
        {
             var context = _msSqlDbExtensionConfigProvider.CreateContext(input);
            
            var sqlSpec = new MsSqlSpec()
            {
                Parameters = input.SqlQueryParameters,
                Query = input.SqlQuery
            };
            
            var list = new List<object>();
#if NETCOREAPP3_1
            var objects = context.MsSqlDbService.GetAsync(sqlSpec, cancellationToken);
           
            await foreach (var o in objects.WithCancellation(cancellationToken))
            {
                list.Add(o);
            }
#else
            var objects = await context.MsSqlDbService.GetAsync(sqlSpec, cancellationToken);
            list.AddRange(objects);
#endif
            return list.AsEnumerable();
        }
    }
}