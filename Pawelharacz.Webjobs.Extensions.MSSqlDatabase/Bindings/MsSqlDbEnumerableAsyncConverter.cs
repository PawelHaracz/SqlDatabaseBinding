using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Config;

namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Bindings
{
    internal class MsSqlDbEnumerableAsyncConverter<T> : IAsyncConverter<MsSqlDbAttribute, IEnumerable<T>>
        where T: class, new()
    {
        private readonly MsSqlDbExtensionConfigProvider _msSqlDbExtensionConfigProvider;

        public MsSqlDbEnumerableAsyncConverter(MsSqlDbExtensionConfigProvider msSqlDbExtensionConfigProvider)
        {
            _msSqlDbExtensionConfigProvider = msSqlDbExtensionConfigProvider;
        }
        
        public async Task<IEnumerable<T>> ConvertAsync(MsSqlDbAttribute input, CancellationToken cancellationToken) 
        {
           var context = _msSqlDbExtensionConfigProvider.CreateContext(input);

           var sqlSpec = new MsSqlSpec()
           {
               Parameters = input.SqlQueryParameters,
               Query = input.SqlQuery
               
           };
           var list = new List<T>();
#if NETCOREAPP3_1
           var objects = context.MsSqlDbService.GetAsync<T>(sqlSpec, cancellationToken);
           
           await foreach (var o in objects.WithCancellation(cancellationToken))
           {
               list.Add(o);
           }
#else
            var objects = await context.MsSqlDbService.GetAsync<T>(sqlSpec, cancellationToken);
            list.AddRange(objects);
#endif
           return list.AsEnumerable();
        }
    }
}