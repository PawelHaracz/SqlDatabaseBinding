using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Pawelharacz.Webjobs.Extensions.MSSqlDatabase;

namespace Pawelharacz.SqlTest
{
    public static class TestFunc
    {
        [FunctionName("TestFunc")]
        public static async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req, 
            [MsSqlDb(SqlQuery = "Select Id, Name from Examples", ConnectionStringSetting = "MsSqlConnectionString")] IEnumerable<Example> examples,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            return  new OkObjectResult(JsonConvert.SerializeObject(examples));
        }
        
        [FunctionName("TestFunc1")]
        public static async Task<IActionResult> RunFilterAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "TestFunc/{id}")]
            HttpRequest req, 
            [MsSqlDb(SqlQuery = "Select Id, Name from Examples where id = {id}", ConnectionStringSetting = "MsSqlConnectionString")] IEnumerable<Example> examples,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            return  new OkObjectResult(JsonConvert.SerializeObject(examples));
        }
        
        [FunctionName("TestFunc2")]
        public static async Task<IActionResult> RunFilterOneAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "TestFunc/one/{id}")]
            HttpRequest req, 
            [MsSqlDb(SqlQuery = "Select Id, Name from Examples where id = {id}", ConnectionStringSetting = "MsSqlConnectionString")] Example example,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            return  new OkObjectResult(JsonConvert.SerializeObject(example));
        }
    }

    public class Example
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}