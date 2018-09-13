
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
    public static class GetRatingH5
    {
        [FunctionName("GetRatingH5")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequest req, ILogger log,
             [CosmosDB(
                databaseName: "openhack",
                collectionName: "ratings",
                ConnectionStringSetting = "CosmosDBConnection", 
                id = "{id}")]Rating ratings
            )
)
            
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string id = req.Query["id"];

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            id = id ?? data?.name;

            return id != null
                ? (ActionResult)new OkObjectResult($"Hello, {id}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
//    public class Rating
 //   {
 //       public string id { get; set; }
 //       public string userId { get; set; }
 //       public string productId { get; set; }
 //       public string timestamp { get; set; }
 //       public string locationName { get; set; }
 //       public string rating { get; set; }
 //       public string userNotes { get; set; }
 //   }
}
