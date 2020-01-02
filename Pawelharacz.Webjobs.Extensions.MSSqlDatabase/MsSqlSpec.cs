using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Pawelharacz.Webjobs.Extensions.MSSqlDatabase
{
    public class MsSqlSpec
    {
        public string Query { get; set; }
        public IEnumerable<SqlParameter> Parameters { get; set; }
    }
}