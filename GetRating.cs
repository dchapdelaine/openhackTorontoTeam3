
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Collections.Generic;

namespace Company.Function
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequest req,
             [CosmosDB(
                databaseName: "openhack",
                collectionName: "ratings",
                ConnectionStringSetting = "CosmosDBConnection", 
                Id = "{Query.id}")]Rating rating, ILogger log
            )
            
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            return (ActionResult)new OkObjectResult (rating);
        }
    }
}