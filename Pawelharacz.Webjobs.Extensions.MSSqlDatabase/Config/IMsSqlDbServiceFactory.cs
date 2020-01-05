using System.Data.SqlClient;
using Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Service;

namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Config
{
    internal interface IMsSqlDbServiceFactory
    {
        IMsSqlDbService CreateService(SqlConnectionStringBuilder connectionString);
    }
}