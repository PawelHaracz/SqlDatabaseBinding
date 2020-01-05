using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Pawelharacz.Webjobs.Extensions.MSSqlDatabase;
using Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Config;
using Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Extensions;

[assembly: WebJobsStartup(typeof(MsSqlDbWebJobsStartup))]
namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase
{
    public class MsSqlDbWebJobsStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddMsSqlDb();
        }
    }
}