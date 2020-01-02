namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Config
{
    internal interface IMsSqlDbServiceFactory
    {
        IMsSqlDbService CreateService(string connectionString);
    }
}