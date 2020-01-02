namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Config
{
    internal class MsSqlDbServiceFactory : IMsSqlDbServiceFactory
    {
        public IMsSqlDbService CreateService(string connectionString)
        {
            return new MsSqlDbService(connectionString);
        }
    }
}