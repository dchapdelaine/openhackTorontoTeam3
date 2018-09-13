
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Documents;
using System.Collections.Generic;

namespace Company.Function
{
    public static class GetRatings
    {
        [FunctionName("GetRatings")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ratings/{id}")]HttpRequest req,
             [CosmosDB(
                databaseName: "openhack",
                collectionName: "ratings",
                ConnectionStringSetting = "CosmosDBConnection", 
                SqlQuery = "SELECT * FROM c WHERE c.userId={id}")]IEnumerable<Rating> ratings, ILogger log
        )
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            return (ActionResult)new OkObjectResult (ratings);
        }
    }
}