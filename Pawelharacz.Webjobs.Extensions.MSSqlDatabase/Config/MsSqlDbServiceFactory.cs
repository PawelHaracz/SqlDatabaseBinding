#if NETCOREAPP3_1
using Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Service.v3;
#else
using Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Service.v2;
#endif
using System.Data.SqlClient;
using Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Service;


namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Config
{
    internal class MsSqlDbServiceFactory : IMsSqlDbServiceFactory
    {
        public IMsSqlDbService CreateService(SqlConnectionStringBuilder connectionString)
        {
            return new MsSqlDbService(connectionString.ToString());
        }
    }
}