using System.Data.SqlClient;
using Microsoft.Azure.WebJobs.Hosting;

namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Config
{
    public class MsSqlDbOptions : IOptionsFormatter
    {
        public string Format()
        {
            //todo should concat a one connection string, if null that means can't be generated from default values
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                return null;
            }
            
            var builder = new SqlConnectionStringBuilder(ConnectionString);

            return builder.ToString();
        }

        public string ConnectionString { get; set; }

    }
}