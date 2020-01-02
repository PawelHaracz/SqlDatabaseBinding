using System;
using System.Data.SqlClient;
using Microsoft.Azure.WebJobs.Hosting;

namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase.Config
{
    public class MsSqlDbOptions : IOptionsFormatter
    {
        public string Format()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                throw new ArgumentNullException(nameof(ConnectionString));
            }
            var builder =   new SqlConnectionStringBuilder(ConnectionString);

            return builder.ToString();
        }

        public string ConnectionString { get; set; }

    }
}