using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Service
{
    internal interface IMsSqlDbService
    {
        IEnumerable<T> Get<T>(MsSqlSpec msSqlSpec) where T : class, new();
        #if NETCOREAPP3_1
        IAsyncEnumerable<T> GetAsync<T>(MsSqlSpec msSqlSpec,
            [EnumeratorCancellation] CancellationToken cancellationToken = default) where T : class, new();

        IAsyncEnumerable<object> GetAsync(MsSqlSpec msSqlSpec,
            [EnumeratorCancellation] CancellationToken cancellationToken = default); 
        #else
            Task<IEnumerable<T>> GetAsync<T>(MsSqlSpec msSqlSpec, CancellationToken cancellationToken = default) where T : class, new();
            Task<IEnumerable<object>> GetAsync(MsSqlSpec msSqlSpec, CancellationToken cancellationToken = default);
        #endif
        
        Task<T> GetOne<T>(MsSqlSpec msSqlSpec,CancellationToken cancellationToken = default) where T : class, new();
    }
}