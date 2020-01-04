using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pawelharacz.Webjobs.Extensions.MSSqlDatabase;
using System.Collections.Generic;
namespace v2
{
    public static class TestFunc
    {
        [FunctionName("TestFunc")]
        public static async Task<IActionResult> Run(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req, 
            [MsSqlDb(SqlQuery = "Select Id, Name from Examples", ConnectionStringSetting = "MsSqlConnectionString")] IEnumerable<Example> examples,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            return  new OkObjectResult(JsonConvert.SerializeObject(examples));
        }

        
    public class Example
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    }
}
