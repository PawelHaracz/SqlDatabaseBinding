using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Config
{
    internal interface IMsSqlDbService
    {
        IEnumerable<T> Get<T>(MsSqlSpec msSqlSpec) where T : class, new();

        IAsyncEnumerable<T> GetAsync<T>(MsSqlSpec msSqlSpec,
            [EnumeratorCancellation] CancellationToken cancellationToken = default) where T : class, new();
    }
}