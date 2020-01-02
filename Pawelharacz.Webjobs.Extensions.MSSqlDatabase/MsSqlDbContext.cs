using Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Config;

namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase
{
    
    internal class MsSqlDbContext
    {
        public MsSqlDbAttribute Attribute { get; set; }
        public IMsSqlDbService MsSqlDbService { get; set; }
    }
}